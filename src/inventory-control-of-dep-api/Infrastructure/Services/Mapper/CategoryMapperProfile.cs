using AutoMapper;

using inventory_control_of_dep_api.Models.Category;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<CreateCategoryRequest, Category>().ReverseMap();
            CreateMap<UpdateCategoryRequest, Category>().ReverseMap(); 
            CreateMap<CategoryResponse, Category>().ReverseMap();
        }
    }
}
