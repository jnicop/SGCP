using AutoMapper;
using SGCP.DTOs.Inventory;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>().ReverseMap();
            CreateMap<InventoryCreateDto, Inventory>()
                            .ForMember(dest => dest.Id, opt => opt.Ignore())
                            .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                            .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                            .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                            .ForMember(dest => dest.Enable, opt => opt.Ignore())
                            .ForMember(dest => dest.InventoryMovements, opt => opt.Ignore())
                            .ForMember(dest => dest.Unit, opt => opt.Ignore());

            CreateMap<InventoryUpdateDto, Inventory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.Ignore())
                .ForMember(dest => dest.EntityTipe, opt => opt.Ignore())
                .ForMember(dest => dest.UnitId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.LocationId, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryMovements, opt => opt.Ignore())
                .ForMember(dest => dest.Unit, opt => opt.Ignore());

        }
    }
}
