using SGCP.DTOs.Catalogs;

namespace SGCP.Services
{
    public interface ICatalogsService
    {
        Task<IEnumerable<CatalogDto>> GetTreatmentTypesAsync();
        Task<IEnumerable<CatalogDto>> GetProcessTypesAsync();
        Task<IEnumerable<CatalogDto>> GetScopeTypesAsync();
        Task<IEnumerable<CatalogDto>> GetComponentTypesAsync();
    }
}
