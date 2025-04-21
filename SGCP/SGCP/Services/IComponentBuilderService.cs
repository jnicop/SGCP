using SGCP.DTOs.Component;

namespace SGCP.Services
{
    public interface IComponentBuilderService
    {
        Task<long> CreateAsync(ComponentBuilderDto dto);
        Task UpdateAsync(ComponentBuilderDto dto);

        Task<ComponentBuilderDto> GetByIdAsync(long id);
        
    }
}
