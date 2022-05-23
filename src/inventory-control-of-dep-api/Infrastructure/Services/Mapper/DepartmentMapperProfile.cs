using AutoMapper;

using inventory_control_of_dep_api.Models.Department;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class DepartmentMapperProfile : Profile
    {
        public DepartmentMapperProfile()
        {
            CreateMap<CreateDepartmentRequest, Department>().ReverseMap();
            CreateMap<UpdateDepartmentRequest, Department>().ReverseMap();
            CreateMap<DepartmentResponse, Department>().ReverseMap();
        }
    }
}
