using AutoMapper;
using SGCP.DTOs.Currency;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<CurrencyCreateDto, Currency>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                        .ForMember(dest => dest.LastUpdate, opt => opt.Ignore())
                        .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                        .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                        .ForMember(dest => dest.Enable, opt => opt.Ignore())
                        .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore());

            CreateMap<CurrencyUpdateDto, Currency>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore());
        }
    }
}
