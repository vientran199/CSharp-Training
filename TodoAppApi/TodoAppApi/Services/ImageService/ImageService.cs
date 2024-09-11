using AutoMapper;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
using TodoAppApi.Dtos.Image;
using TodoAppApi.Models;

namespace TodoAppApi.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly MyDataContext _context;
        private readonly IMapper _mapper;

        public ImageService(IWebHostEnvironment environment, MyDataContext context, IMapper mapper) 
        { 
            _environment = environment;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> add(IFormFile image, int categoryId)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
            {
                throw new Exception($"Category not found {categoryId}");
            }

            byte[] bytes = convertImageToByteArray(image);
            
            _context.Images.Add(
                new Models.Image
                {
                    Name = image.FileName,
                    ImageContent = bytes,
                    CategoryId = categoryId,
                }
            );

            await _context.SaveChangesAsync();


            return true;
        }

        public async Task<List<GetImageDto>> getAll(int categoryId)
        {
            var images = _context.Images.Include(i => i.Category).AsQueryable();

            if(Convert.ToBoolean(categoryId))
            {
                images = images.Where(c => c.CategoryId == categoryId);
            }
            
            return images.Select(i => _mapper.Map<GetImageDto>(i)).ToList();
        }

        public async Task<bool> update(int id, IFormFile image, int categoryId)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
            {
                throw new Exception($"Category not found id {categoryId}");
            }

            var _image = _context.Images.SingleOrDefault(i => i.Id == id);

            if (_image == null)
            {
                throw new Exception($"Image not found id {id}");
            }

            long length = image.Length;
            if (length < 0)
            {
                return false;
            }

            using var fileStream = image.OpenReadStream();
            byte[] bytes = new byte[length];
            fileStream.Read(bytes, 0, (int)image.Length);

            _image.Name = image.FileName;
            _image.ImageContent = bytes;
            _image.CategoryId = categoryId;

            await _context.SaveChangesAsync();

            return true;
        }

        private string getFilePath()
        {
            return _environment.WebRootPath + "\\Uploads";
        }

        private byte[] convertImageToByteArray(IFormFile image)
        {
            
            long length = image.Length;

            byte[] bytes = new byte[length];

            if (length < 0)
            {
                return bytes;
            }

            using (var fileStream = image.OpenReadStream())

            //Example: https://github.com/dlemstra/Magick.NET/blob/main/samples/Magick.NET.Samples/ConvertImage.cs
            {
                MagickImage image1 = new MagickImage(fileStream);

                //image1.Format = image1.Format; // Get or Set the format of the image.
                image1.Resize(800, 600); // fit the image into the requested width and height. 
                //image1.Quality = 10; // This is the Compression level.

                bytes = image1.ToByteArray(); //.Read(bytes, 0, (int)image.Length);
            }

            return bytes;
        }
    }
}
