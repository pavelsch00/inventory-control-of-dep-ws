using AutoMapper;

using inventory_control_of_dep_api.Models.Faculty;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class FacultyMapperProfile : Profile
    {
        public FacultyMapperProfile()
        {
            CreateMap<CreateFacultyRequest, Faculty>().ReverseMap();
            CreateMap<UpdateFacultyRequest, Faculty>().ReverseMap();
            CreateMap<FacultyResponse, Faculty>().ReverseMap();
        }
    }
}
