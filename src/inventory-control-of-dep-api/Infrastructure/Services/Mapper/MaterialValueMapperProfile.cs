using AutoMapper;

using inventory_control_of_dep_api.Models.MaterialValue;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class MaterialValueMapperProfile : Profile
    {
        public MaterialValueMapperProfile()
        {
            CreateMap<CreateMaterialValueRequest, MaterialValue>().ReverseMap();
            CreateMap<UpdateMaterialValueRequest, MaterialValue>().ReverseMap();
            CreateMap<MaterialValueResponse, MaterialValue>().ReverseMap();
        }
    }
}
