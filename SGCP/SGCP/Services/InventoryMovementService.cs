using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.InventoryMovement;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class InventoryMovementService : IInventoryMovementService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<InventoryMovementService> _logger;

        public InventoryMovementService(SGCP_DbContext context, IMapper mapper, ILoggingService<InventoryMovementService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<InventoryMovementDto>> GetAllAsync()
        {
            _logger.Info("Getting all inventory movements.");
            var list = await _context.InventoryMovements.Where(m => m.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<InventoryMovementDto>>(list);
        }

        public async Task<InventoryMovementDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting inventory movement with ID {Id}.", id);
                var item = await _context.InventoryMovements.FirstOrDefaultAsync(m => m.Id == id && m.Enable == true);
                if (item == null)
                    throw new KeyNotFoundException($"Inventory movement with ID {id} not found.");

                return _mapper.Map<InventoryMovementDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting inventory movement with ID {Id}.", id);
                throw;
            }
        }

        public async Task<InventoryMovementDto> CreateAsync(InventoryMovementCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.Id == dto.InventoryId);

                if (inventory == null)
                    throw new InvalidOperationException("Inventory not found.");

                if (dto.MovementType == "In")
                {
                    inventory.Quantity += dto.Quantity;
                }
                else if (dto.MovementType == "Out")
                {
                    if (inventory.Quantity < dto.Quantity)
                        throw new InvalidOperationException("Not enough stock.");
                    inventory.Quantity -= dto.Quantity;
                }
                else
                {
                    throw new InvalidOperationException("Invalid MovementType. Use 'In' or 'Out'.");
                }

                var movement = _mapper.Map<InventoryMovement>(dto);
                movement.Date = DateTime.UtcNow;
                movement.InsertDate = DateTime.UtcNow;
                movement.Enable = true;

                _context.InventoryMovements.Add(movement);
                _context.Inventories.Update(inventory);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.Info($"InventoryMovement registered: {dto.MovementType} {dto.Quantity} for InventoryId {dto.InventoryId}");

                return _mapper.Map<InventoryMovementDto>(movement);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error(ex, "Error while creating inventory movement.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, InventoryMovementUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating inventory movement with ID {Id}.", id);
                var existing = await _context.InventoryMovements.FindAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException($"Inventory movement with ID {id} not found.");

                _mapper.Map(dto, existing);
                existing.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating inventory movement with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var movement = await _context.InventoryMovements
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (movement == null)
                    return false;

                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.Id == movement.InventoryId);

                if (inventory == null)
                    throw new InvalidOperationException("Related inventory not found.");

                // Revertir el efecto del movimiento
                if (movement.MovementType == "In")
                {
                    if (inventory.Quantity < movement.Quantity)
                        throw new InvalidOperationException("Cannot delete movement: insufficient stock to revert.");
                    inventory.Quantity -= movement.Quantity;
                }
                else if (movement.MovementType == "Out")
                {
                    inventory.Quantity += movement.Quantity;
                }

                _context.InventoryMovements.Remove(movement);
                _context.Inventories.Update(inventory);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.Info($"InventoryMovement {id} deleted and inventory stock updated.");

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error(ex, $"Error deleting inventory movement {id}.");
                throw;
            }
        }

    }
}
