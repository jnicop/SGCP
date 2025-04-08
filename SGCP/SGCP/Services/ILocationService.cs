using SGCP.DTOs.Location;

namespace SGCP.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationDto>> GetAllAsync();
        Task<LocationDto> GetByIdAsync(long id);
        Task<LocationDto> CreateAsync(LocationCreateDto dto);
        Task<bool> UpdateAsync(long id, LocationUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
