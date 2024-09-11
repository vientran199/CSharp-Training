using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoAppApi.Data;
using TodoAppApi.Models;
using TodoAppApi.Services.CategoryService;
using TodoAppApi.Services.ImageService;
using TodoAppApi.Services.JobService;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddDbContext<MyDataContext>(); //Add to use DbContext, Add first

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add mapper to assemply, must place on top of Service
builder.Services.AddAutoMapper(typeof(Program).Assembly); //To use AutoMapper

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<ICategoryService, MockCategoryService>();

//Configuration để sử dụng model AppSetting, khi đó nó sẽ mapp với giá trị của properties AppSetting trong file AppSetting.json
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

//Configuration Authentication
var secretKet = builder.Configuration["AppSetting:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKet);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            //Tự cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            //Kys vaof token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes), //Sử dụng thuật toán này là đối xứng, nó sẽ tự động mã hóa

            ClockSkew = TimeSpan.Zero
        };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); //Xac thuc
app.UseAuthorization(); //Phan quyen

app.MapControllers();

app.Run();
