using AutoMapper;
using SGCP.DTOs.ProductComponent;
using SGCP.Models;

public class ProductComponentProfile : Profile
{
    public ProductComponentProfile()
    {
        // ProductComponent → ProductComponentDto
        CreateMap<ProductComponent, ProductComponentDto>()
            .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
           .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
            //.ForMember(dest => dest.ComponentName, opt => opt.MapFrom(src => src.Component != null ? src.Component.Name : ""))
            .ForMember(dest => dest.InsertDate, opt => opt.MapFrom(src => src.InsertDate ?? DateTime.MinValue))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
            .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => src.Enable))
            .ReverseMap() // Mapea de vuelta de DTO → Entidad
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.Component, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
            .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
            .ForMember(dest => dest.Enable, opt => opt.Ignore());

        // Create DTO
        CreateMap<ProductComponentCreateDto, ProductComponent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.Unit, opt => opt.Ignore())
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId)) // si UnitId está en el DTO
            .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
            .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
            .ForMember(dest => dest.Enable, opt => opt.Ignore())
            .ForMember(dest => dest.Component, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());

        // Update DTO
        CreateMap<ProductComponentUpdateDto, ProductComponent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.Unit, opt => opt.Ignore())
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId)) // si UnitId está en el DTO
            .ForMember(dest => dest.ComponentId, opt => opt.Ignore())
            .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
            .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
            .ForMember(dest => dest.Component, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());
    }
}
