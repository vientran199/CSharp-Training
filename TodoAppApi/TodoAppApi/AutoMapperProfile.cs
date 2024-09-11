using AutoMapper;
using TodoAppApi.Dtos.Category;
using TodoAppApi.Dtos.Image;
using TodoAppApi.Models;

namespace TodoAppApi
{
    //Here config AutoMapper, to active
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Category, GetCategoryDto>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<Image, GetImageDto>();
        }
    }
}
