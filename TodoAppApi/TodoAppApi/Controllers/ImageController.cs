using Microsoft.AspNetCore.Mvc;
using TodoAppApi.Dtos.Image;
using TodoAppApi.Models;
using TodoAppApi.Services.ImageService;

namespace TodoAppApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService) 
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<List<GetImageDto>> getAll([FromQuery] int categoryId)
        {
            var request = Request;

            return await _imageService.getAll(categoryId);
        }

        [HttpPost]
        [RequestSizeLimit(200000000)]
        //[DisableRequestSizeLimit]
        public async Task<IActionResult> add([FromForm] IFormFile image, [FromForm] int categoryId)
        {
            var a = Request;

            bool result = await _imageService.add(image, categoryId);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> update([FromRoute] int id, [FromForm] IFormFile image, [FromForm] int categoryId)
        {
            var a = Request;

            bool result = await _imageService.update(id, image, categoryId);
            return Ok(result);
        }
    }
}
