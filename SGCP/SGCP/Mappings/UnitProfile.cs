using AutoMapper;
using SGCP.DTOs.Unit;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            CreateMap<Unit, UnitDto>().ReverseMap();
            CreateMap<UnitCreateDto, Unit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.Components, opt => opt.Ignore())
                .ForMember(dest => dest.Inventories, opt => opt.Ignore())
                .ForMember(dest => dest.LaborCosts, opt => opt.Ignore())
                .ForMember(dest => dest.LaborTypes, opt => opt.Ignore())
                .ForMember(dest => dest.ProductComponents, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentPresentations, opt => opt.Ignore())
                .ForMember(dest => dest.ProductPackagings, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<UnitUpdateDto, Unit>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Components, opt => opt.Ignore())
                .ForMember(dest => dest.Inventories, opt => opt.Ignore())
                .ForMember(dest => dest.LaborCosts, opt => opt.Ignore())
                .ForMember(dest => dest.LaborTypes, opt => opt.Ignore())
                .ForMember(dest => dest.ProductComponents, opt => opt.Ignore())
                 .IncludeBase<UnitCreateDto, Unit>();   
        }
    }
}