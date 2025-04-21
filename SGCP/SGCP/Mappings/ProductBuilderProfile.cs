using AutoMapper;
using SGCP.DTOs.Product;
using SGCP.DTOs.ProductComponent;
using SGCP.DTOs.LaborCost;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class ProductBuilderProfile : Profile
    {
        public ProductBuilderProfile()
        {
            CreateMap<ProductBuilderDto, Product>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                  //.ForMember(dest => dest.ProductTypeId, opt => opt.MapFrom(src => src.ProductTypeId))
                  .ForMember(dest => dest.ProductComponents, opt => opt.Ignore())
                  .ForMember(dest => dest.LaborCosts, opt => opt.Ignore())
                  .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore())
                  .ForMember(dest => dest.Category, opt => opt.Ignore())
                  .ForMember(dest => dest.FinalPrice, opt => opt.Ignore())
                  .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                  .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                  .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                  .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                  .ForMember(dest => dest.Enable, opt => opt.Ignore())
                    .ForMember(dest => dest.ProductPrice, opt => opt.Ignore())
                        .ForMember(dest => dest.Presentation, opt => opt.MapFrom(src => src.Presentation))
    .ForMember(dest => dest.TotalLength, opt => opt.MapFrom(src => src.TotalLength))
    .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
    .ForMember(dest => dest.ProductFixedCosts, opt => opt.Ignore())
    .ForMember(dest => dest.ProductPackagings, opt => opt.Ignore())
    .ForMember(dest => dest.Unit, opt => opt.Ignore());

            // Mapear entidad a DTO
            //CreateMap<Product, ProductBuilderDto>()
            //    .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Components, opt => opt.MapFrom(src => src.ProductComponents))
            //    .ForMember(dest => dest.LaborCosts, opt => opt.MapFrom(src => src.LaborCosts));

            // 🔥 Agregar este para evitar el error actual
            CreateMap<Product, ProductBuilderDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                //.ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.FinalPrice))
                .ForMember(dest => dest.Components, opt => opt.MapFrom(src => src.ProductComponents))
                .ForMember(dest => dest.LaborCosts, opt => opt.MapFrom(src => src.LaborCosts));

        }
    }
}
