using AutoMapper;
using SGCP.DTOs.Product;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Modelo <-> DTO principal
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ReverseMap();

            // CreateDto -> Modelo
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                //.ForMember(dest => dest.ProductTypeId, opt => opt.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.FinalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.LaborCosts, opt => opt.Ignore())
                .ForMember(dest => dest.ProductComponents, opt => opt.Ignore())
                .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ProductPrice, opt => opt.Ignore());

            // UpdateDto -> Modelo
            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.FinalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.LaborCosts, opt => opt.Ignore())
                .ForMember(dest => dest.ProductComponents, opt => opt.Ignore())
                .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ProductPrice, opt => opt.Ignore());
        }
    }
}
