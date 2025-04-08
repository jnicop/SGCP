using AutoMapper;
using SGCP.DTOs.Region;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class RegionProfile:Profile
    {
        public RegionProfile()
        {
            CreateMap<RegionCreateDto, Region>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
            .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
            .ForMember(dest => dest.Enable, opt => opt.Ignore())
            .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore());

            CreateMap<RegionUpdateDto, Region>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.RegionalsPrices, opt => opt.Ignore());
        }
    }
}
