using AutoMapper;

using inventory_control_of_dep_api.Models.User;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<ChangeUserPasswordRequest, User>().ReverseMap();
            CreateMap<UpdateUserRequest, User>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();
            CreateMap<AddUserRoleRequest, User>().ReverseMap();
        }
    }
}
