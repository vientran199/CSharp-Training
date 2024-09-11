using Microsoft.AspNetCore.Mvc;
using TodoAppApi.Dtos.Category;
using TodoAppApi.Models;
using TodoAppApi.Services.CategoryService;

namespace TodoAppApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryDto>>>> GetAll()
        {
            return await _categoryService.getAll();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> GetById(int id)
        {
            return await _categoryService.getById(id);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> Create(AddCategoryDto category)
        {
            return await _categoryService.create(category);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> Update(int id, UpdateCategoryDto category)
        {
            return await _categoryService.update(id, category);
        }
    }
}
