using SGCP.DTOs.Inventory;

namespace SGCP.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllAsync();
        Task<InventoryDto> GetByIdAsync(long id);
        Task<InventoryDto> CreateAsync(InventoryCreateDto dto);
        Task<bool> UpdateAsync(long id, InventoryUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
