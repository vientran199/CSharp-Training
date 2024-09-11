using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Dtos.Category;
using TodoAppApi.Models;

namespace TodoAppApi.Services.CategoryService
{
    public class MockCategoryService : ICategoryService
    {
        private readonly List<Category> _categories = new List<Category>();
        
        private readonly IMapper _mapper;

        public MockCategoryService(IMapper mapper, List<Category>? categories) 
        { 
            _mapper = mapper;
            _categories = categories is null ? new List<Category>() : categories;
        }

        public async Task<ServiceResponse<GetCategoryDto>> create(AddCategoryDto category)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            Category categoryMap = _mapper.Map<Category>(category);
            categoryMap.Id = _getLastId() + 1;

            _categories.Add(categoryMap);

            response.Data = _mapper.Map<GetCategoryDto>(categoryMap);
            return response;
        }

        public async Task<ServiceResponse<List<GetCategoryDto>>> getAll()
        {
            var response = new ServiceResponse<List<GetCategoryDto>>();

            response.Data = _categories.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();

            return response;
        }

        public async Task<ServiceResponse<GetCategoryDto>> getById(int id)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            try
            {
                var category = _categories.SingleOrDefault(c => c.Id == id);

                if (category is null)
                {
                    throw new Exception($"Not found {id}");
                }

                response.Data = _mapper.Map<GetCategoryDto>(category);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetCategoryDto>> update(int id, UpdateCategoryDto category)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            try
            {
                var _category = _categories.SingleOrDefault(c => c.Id == id);
                if (_category == null)
                {
                    throw new Exception($"Not found {id}");
                }

                _category.Name = category.Name;

                response.Data = _mapper.Map<GetCategoryDto>(_category);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private int _getLastId()
        {
            if(_categories.Count == 0)
            {
                return 0;
            }

            return _categories.Max(c => c.Id);
        }
    }
}
