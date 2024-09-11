using TodoAppApi.Dtos.Category;
using TodoAppApi.Models;

namespace TodoAppApi.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<GetCategoryDto>>> getAll();
        Task<ServiceResponse<GetCategoryDto>> getById(int id);
        Task<ServiceResponse<GetCategoryDto>> create(AddCategoryDto category);

        Task<ServiceResponse<GetCategoryDto>> update(int id, UpdateCategoryDto category);
    }
}
