using SGCP.DTOs.InventoryMovement;

namespace SGCP.Services
{
    public interface IInventoryMovementService
    {
        Task<IEnumerable<InventoryMovementDto>> GetAllAsync();
        Task<InventoryMovementDto> GetByIdAsync(long id);
        Task<InventoryMovementDto> CreateAsync(InventoryMovementCreateDto dto);
        Task<bool> UpdateAsync(long id, InventoryMovementUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
