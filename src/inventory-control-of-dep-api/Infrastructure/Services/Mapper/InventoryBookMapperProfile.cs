using AutoMapper;

using inventory_control_of_dep_api.Models.InventoryBook;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.Mapper
{
    public class InventoryBookMapperProfile : Profile
    {
        public InventoryBookMapperProfile()
        {
            CreateMap<CreateInventoryBookRequest, InventoryBook>().ReverseMap();
            CreateMap<UpdateInventoryBookRequest, InventoryBook>().ReverseMap();
            CreateMap<InventoryBookResponse, InventoryBook>().ReverseMap();
        }
    }
}
