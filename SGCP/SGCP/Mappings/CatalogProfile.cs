using AutoMapper;
using SGCP.DTOs.Catalogs;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class CatalogProfile: Profile
    {
        public CatalogProfile()
        {
            CreateMap<TreatmentType, CatalogDto>();
            CreateMap<ProcessType, CatalogDto>();
            CreateMap<ScopeType, CatalogDto>();
            CreateMap<ComponentType, CatalogDto>();
            CreateMap<Unit, CatalogDto>();
        }
    }
}
