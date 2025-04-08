using AutoMapper;
using SGCP.DTOs.RegionalsPrice;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class RegionalsPriceProfile : Profile
    {
        public RegionalsPriceProfile()
        {
            CreateMap<RegionalsPrice, RegionalsPriceDto>().ReverseMap();
            CreateMap<RegionalsPriceCreateDto, RegionalsPrice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore());

            CreateMap<RegionalsPriceUpdateDto, RegionalsPrice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.RegionId, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore());
        }
    }
}
