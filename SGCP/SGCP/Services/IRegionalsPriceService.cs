using SGCP.DTOs.RegionalsPrice;

namespace SGCP.Services
{
    public interface IRegionalsPriceService
    {
        Task<IEnumerable<RegionalsPriceDto>> GetAllAsync();
        Task<RegionalsPriceDto> GetByIdAsync(long id);
        Task<RegionalsPriceDto> CreateAsync(RegionalsPriceCreateDto dto);
        Task<bool> UpdateAsync(long id, RegionalsPriceUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
