using AutoMapper;

using inventory_control_of_dep_api.Models.Position;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class PositionMapperProfile : Profile
    {
        public PositionMapperProfile()
        {
            CreateMap<CreatePositionRequest, Position>().ReverseMap();
            CreateMap<UpdatePositionRequest, Position>().ReverseMap();
            CreateMap<PositionResponse, Position>().ReverseMap();
        }
    }
}
