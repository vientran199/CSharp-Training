using TodoAppApi.Dtos.Image;
using TodoAppApi.Models;

namespace TodoAppApi.Services.ImageService
{
    public interface IImageService
    {
        Task<bool> add(IFormFile image, int categoryId);
        Task<bool> update(int id, IFormFile image, int categoryId);

        Task<List<GetImageDto>> getAll(int categoryId);
    }
}
