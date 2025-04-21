using AutoMapper;
using SGCP.DTOs.InventoryMovement;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class InventoryMovementProfile : Profile
    {
        public InventoryMovementProfile()
        {
            CreateMap<InventoryMovement, InventoryMovementDto>().ReverseMap();
            CreateMap<InventoryMovementCreateDto, InventoryMovement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.Inventory, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<InventoryMovementUpdateDto, InventoryMovement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Inventory, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());
        }
    }
}
