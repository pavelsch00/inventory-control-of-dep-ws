using AutoMapper;

using inventory_control_of_dep_api.Models.Aprovar;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class AprovarMapperProfile : Profile
    {
        public AprovarMapperProfile()
        {
            CreateMap<CreateAprovarRequest, Aprovar>().ReverseMap();
            CreateMap<UpdateAprovarRequest, Aprovar>().ReverseMap();
            CreateMap<AprovarResponse, Aprovar>().ReverseMap();
        }
    }
}
