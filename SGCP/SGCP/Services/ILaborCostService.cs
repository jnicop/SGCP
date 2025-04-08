using SGCP.DTOs.LaborCost;

namespace SGCP.Services
{
    public interface ILaborCostService
    {
        Task<IEnumerable<LaborCostDto>> GetAllAsync();
        Task<LaborCostDto> GetByIdAsync(long id);
        Task<LaborCostDto> CreateAsync(LaborCostCreateDto dto);
        Task<bool> UpdateAsync(long id, LaborCostUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
