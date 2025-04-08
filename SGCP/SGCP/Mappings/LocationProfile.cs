using AutoMapper;
using SGCP.DTOs.Location;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<LocationCreateDto, Location>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryMovements, opt => opt.Ignore());

            CreateMap<LocationUpdateDto, Location>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryMovements, opt => opt.Ignore());
        }
    }
}
