using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Data;
using TodoAppApi.Dtos.Category;
using TodoAppApi.Models;

namespace TodoAppApi.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly MyDataContext _context;
        private readonly IMapper _mapper;
        public CategoryService(MyDataContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCategoryDto>> create(AddCategoryDto category)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            var categories = _context.Categories;

            var newCategory = _mapper.Map<Category>(category);

            categories.Add(newCategory);
            await _context.SaveChangesAsync();

            var categoryResult = _mapper.Map<GetCategoryDto>(newCategory);

            response.Data = categoryResult;
            return response;
        }

        public async Task<ServiceResponse<List<GetCategoryDto>>> getAll()
        {
            var response = new ServiceResponse<List<GetCategoryDto>>();
            var categories = await _context.Categories.ToListAsync();
            response.Data = categories.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCategoryDto>> getById(int id)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    throw new Exception($"Not found {id}");
                }

                response.Data = _mapper.Map<GetCategoryDto>(category);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public async Task<ServiceResponse<GetCategoryDto>> update(int id, UpdateCategoryDto category)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            var _category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if(_category == null)
            {
                response.Success = false;
                response.Message = $"Not found {id}";

                return response;
            }

            _category.Name = category.Name;
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetCategoryDto>(_category);
            return response;
        }
    }
}
