using SGCP.DTOs.Currency;

namespace SGCP.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto> GetByIdAsync(long id);
        Task<CurrencyDto> CreateAsync(CurrencyCreateDto dto);
        Task<bool> UpdateAsync(long id, CurrencyUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
