using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodoAppApi.Data;
using TodoAppApi.Models;

namespace TodoAppApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDataContext _context;
        private readonly AppSetting _setting;

        public UserController(MyDataContext context, IOptionsMonitor<AppSetting> optionsMoniter) 
        {
            _context = context;
            _setting = optionsMoniter.CurrentValue;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Validate(LoginModel model)
        {
            var response = new ServiceResponse<TokenModel>();
            var user = _context.Users.SingleOrDefault(u => u.UserName == model.UserName);

            if(user == null)
            {
                response.Success = false;
                response.Message = "Invalid username/password";
                return Ok(response);
            }
            //Cap token
            var token = await _GenerateToken(user);

            response.Data = token;
            return Ok(response);    
        }

        private async Task<TokenModel> _GenerateToken(User user )
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_setting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                     new Claim(ClaimTypes.Name, user.Name),
                     new Claim(JwtRegisteredClaimNames.Email, user.Email),
                     new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //Cai nay no se laf ID cua token
                     new Claim("UserName", user.UserName),
                     new Claim("Id", user.Id.ToString()),

                     //roles

                }),
                Expires = DateTime.UtcNow.AddMinutes(1), //Quy ve UTC

                //Ham ky
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes), 
                    SecurityAlgorithms.HmacSha256Signature
                    ),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken =  jwtTokenHandler.WriteToken(token);
            var refreshToken = _GenerateRefreshToken();

            //Luu refresh token vao db

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                Token = refreshToken,
                UserId = user.Id,
                IsUsed = false,
                IsRevoked = false,
                IssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(3),
            };

            _context.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string _GenerateRefreshToken()
        {
            var random = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        [HttpPost]
        [Route("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_setting.SecretKey);

            var tokenValidateParam = new TokenValidationParameters
            {
                //Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //Kys vaof token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes), //Sử dụng thuật toán này là đối xứng, nó sẽ tự động mã hóa

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false, //Khong check expired
            };

            try
            {
                //Check 1: Token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(
                    model.AccessToken,
                    tokenValidateParam,
                    out var validatedToken);

                //Check 2: check alg, check thuat toan luc generate co dung khong
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase); //Khong phan biet chu hoa thuong

                    if(!result)
                    {
                        return Ok(new ServiceResponse<TokenModel>
                        {
                            Message = "Invalid Token",
                            Success = false,
                        });
                    }
                }

                //Check 3: Chek expired
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if(expireDate > DateTime.UtcNow)
                {
                    return Ok(new ServiceResponse<TokenModel>
                    {
                        Message = "Token chua expired",
                        Success = false,
                    });
                }

                //Check 4: Check refresh token co trong DB khong
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);

                if(storedToken is null)
                {
                    return Ok(new ServiceResponse<TokenModel>
                    {
                        Message = "Refresh token not found",
                        Success = false,
                    });
                }

                //Check 5: check refresh token isUsed/revoked

                if(storedToken.IsUsed || storedToken.IsRevoked)
                {
                    return Ok(new ServiceResponse<TokenModel>
                    {
                        Message = "Da su dung",
                        Success = false,
                    });
                }

                //Check 6: AccessTOken id == JwtId in refreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                if(storedToken.JwtId != jti)
                {
                    return Ok(new ServiceResponse<TokenModel>
                    {
                        Message = "Not match",
                        Success = false,
                    });
                }

                //Update token is used
                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;

                await _context.SaveChangesAsync();

                //Renew
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == storedToken.UserId);

                if(user is null)
                {
                    return Ok(new ServiceResponse<TokenModel>
                    {
                        Message = "user not found",
                        Success = false,
                    });
                }

                var newToken = await _GenerateToken(user);


                return Ok(new ServiceResponse<TokenModel>
                {
                    Message = "Renew successfully",
                    Data = newToken,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<TokenModel>
                {
                    Message = "Something went wrong",
                    Success = false,
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
