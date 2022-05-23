using Microsoft.AspNetCore.Identity;

using AutoMapper;

using inventory_control_of_dep_api.Models.Authorization;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class AuthorizationMapperProfile : Profile
    {
        public AuthorizationMapperProfile()
        {
            CreateMap<User, RegistrationRequest>().ReverseMap();
        }
    }
}
