using AutoMapper;
using SGCP.DTOs.Component;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class ComponentProfile : Profile
    {
        public ComponentProfile()
        {
            // DTO para visualización general
            CreateMap<Component, ComponentDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ReverseMap();

            // DTO para alta
            CreateMap<ComponentCreateDto, Component>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserInsert, opt => opt.Ignore())
                .ForMember(dest => dest.UserUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.Enable, opt => opt.Ignore())
                .ForMember(dest => dest.ProductComponents, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Unit, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentAttributes, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentPresentations, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentProcesses, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentTreatments, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentType, opt => opt.Ignore())
                .ForMember(dest => dest.ProductPackagings, opt => opt.Ignore());

            // DTO para edición
            CreateMap<ComponentUpdateDto, Component>()
                .IncludeBase<ComponentCreateDto, Component>();

            // DTO completo para crear y editar (builder)
            CreateMap<ComponentBuilderDto, Component>()
                .IncludeBase<ComponentCreateDto, Component>();

            // DTO para leer un componente completo
            CreateMap<Component, ComponentBuilderDto>()
                .ForMember(dest => dest.Presentations, opt => opt.MapFrom(src => src.ComponentPresentations))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.ComponentAttributes))
                .ForMember(dest => dest.Treatments, opt => opt.MapFrom(src => src.ComponentTreatments))
                .ForMember(dest => dest.Processes, opt => opt.MapFrom(src => src.ComponentProcesses));

            // Subentidades: Presentaciones
            CreateMap<ComponentPresentationDto, ComponentPresentation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.Component, opt => opt.Ignore())
                .ForMember(dest => dest.Unit, opt => opt.Ignore());

            CreateMap<ComponentPresentation, ComponentPresentationDto>();

            // Subentidades: Atributos
            CreateMap<ComponentAttributeDto, ComponentAttribute>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.Component, opt => opt.Ignore());

            CreateMap<ComponentAttribute, ComponentAttributeDto>();

            // Subentidades: Tratamientos
            CreateMap<ComponentTreatmentDto, ComponentTreatment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.InsertDate, opt => opt.Ignore())
                .ForMember(dest => dest.Component, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentType, opt => opt.Ignore());

            CreateMap<ComponentTreatment, ComponentTreatmentDto>();

            // Subentidades: Procesos
            CreateMap<ComponentProcessDto, ComponentProcess>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ComponentId, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore()) // columna calculada
                .ForMember(dest => dest.Component, opt => opt.Ignore())
                .ForMember(dest => dest.ProcessType, opt => opt.Ignore())
                .ForMember(dest => dest.ScopeType, opt => opt.Ignore());

            CreateMap<ComponentProcess, ComponentProcessDto>();
        }
    }
}
