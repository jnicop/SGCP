using SGCP.DTOs.Component;

namespace SGCP.Services
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentDto>> GetAllAsync();
        Task<ComponentDto> GetByIdAsync(long id);
        Task<ComponentDto> CreateAsync(ComponentCreateDto dto);
        Task<bool> UpdateAsync(long id, ComponentUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
