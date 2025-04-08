using SGCP.DTOs.Unit;
using SGCP.Services;

namespace SGCP.Services
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDto>> GetAllAsync();
        Task<UnitDto> GetByIdAsync(long id);
        Task<UnitDto> CreateAsync(UnitCreateDto dto);
        Task<bool> UpdateAsync(long id, UnitUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
