using AutoMapper;
using SGCP.DTOs.Category;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Mapeo explícito de Category → CategoryDto
            CreateMap<Category, CategoryDto>()
    .ForMember(dest => dest.Type, opt => opt.Ignore());


            // Mapeo explícito de CategoryDto → Category
            CreateMap<CategoryDto, Category>()
                    .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                    .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                    .ForMember(dest => dest.Components, opt => opt.Ignore())
                    .ForMember(dest => dest.Products, opt => opt.Ignore())
                        .ForMember(dest => dest.CategoryTypeId, opt => opt.Ignore())
        .ForMember(dest => dest.CategoryType, opt => opt.Ignore());

            // Create
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.Components, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                    .ForMember(dest => dest.CategoryTypeId, opt => opt.Ignore())
    .ForMember(dest => dest.CategoryType, opt => opt.Ignore());

            // Update
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Components, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryTypeId, opt => opt.Ignore())
    .ForMember(dest => dest.CategoryType, opt => opt.Ignore());

        }
    }
}
