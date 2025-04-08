using SGCP.DTOs.Region;

namespace SGCP.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDto>> GetAllAsync();
        Task<RegionDto> GetByIdAsync(long id);
        Task<RegionDto> CreateAsync(RegionCreateDto dto);
        Task<bool> UpdateAsync(long id, RegionUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
