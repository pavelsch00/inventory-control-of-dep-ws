using AutoMapper;

using inventory_control_of_dep_api.Models.Room;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class RoomMapperProfile : Profile
    {
        public RoomMapperProfile()
        {
            CreateMap<CreateRoomRequest, Room>().ReverseMap();
            CreateMap<UpdateRoomRequest, Room>().ReverseMap();
            CreateMap<RoomResponse, Room>().ReverseMap();
        }
    }
}
