using AutoMapper;
using SGCP.DTOs.LaborCost;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class LaborCostProfile : Profile
    {
        public LaborCostProfile()
        {
            // LaborCost → LaborCostDto
            CreateMap<LaborCost, LaborCostDto>()
                .ForMember(dest => dest.LaborTypeId, opt => opt.MapFrom(src => src.LaborTypeId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                //.ForMember(dest => dest.LaborTypeName, opt => opt.MapFrom(src => src.LaborType != null ? src.LaborType.Name : ""))
                //.ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
                //.ForMember(dest => dest.HoursCost, opt => opt.MapFrom(src => src.LaborType != null ? src.LaborType.HourlyCost : 0))
                .ForMember(dest => dest.InsertDate, opt => opt.MapFrom(src => src.InsertDate ?? DateTime.MinValue))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => src.Enable));

            // LaborCostDto → LaborCost (solo campos editables)
            CreateMap<LaborCostDto, LaborCost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LaborType, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Unit, opt => opt.Ignore());

            // LaborCostCreateDto → LaborCost
            CreateMap<LaborCostCreateDto, LaborCost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.LaborType, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Unit, opt => opt.Ignore());

            // LaborCostUpdateDto → LaborCost
            CreateMap<LaborCostUpdateDto, LaborCost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.LaborTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.LaborType, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Unit, opt => opt.Ignore());
        }
    }
}
