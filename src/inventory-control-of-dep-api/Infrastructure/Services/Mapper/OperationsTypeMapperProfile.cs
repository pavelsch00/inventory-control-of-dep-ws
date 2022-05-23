using AutoMapper;

using inventory_control_of_dep_api.Models.OperationsType;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class OperationsTypeMapperProfile : Profile
    {
        public OperationsTypeMapperProfile()
        {
            CreateMap<CreateOperationsTypeRequest, OperationsType>().ReverseMap();
            CreateMap<UpdateOperationsTypeRequest, OperationsType>().ReverseMap();
            CreateMap<OperationsTypeResponse, OperationsType>().ReverseMap();
        }
    }
}
