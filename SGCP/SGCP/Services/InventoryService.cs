using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Inventory;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<InventoryService> _logger;

        public InventoryService(SGCP_DbContext context, IMapper mapper, ILoggingService<InventoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllAsync()
        {
            _logger.Info("Getting all inventories.");
            var items = await _context.Inventories.Where(i => i.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<InventoryDto>>(items);
        }

        public async Task<InventoryDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting inventory with ID {Id}.", id);
                var item = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == id && i.Enable == true);
                if (item == null)
                    throw new KeyNotFoundException($"Inventory with ID {id} not found.");

                return _mapper.Map<InventoryDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting inventory with ID {Id}.", id);
                throw;
            }
        }

        public async Task<InventoryDto> CreateAsync(InventoryCreateDto dto)
        {
            try
            {
                _logger.Info("Creating new inventory.");
                var entity = _mapper.Map<Inventory>(dto);
                entity.InsertDate = DateTime.UtcNow;
                entity.Enable = true;

                _context.Inventories.Add(entity);
                await _context.SaveChangesAsync();

                return _mapper.Map<InventoryDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating inventory.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, InventoryUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating inventory with ID {Id}.", id);
                var existing = await _context.Inventories.FindAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException($"Inventory with ID {id} not found.");

                _mapper.Map(dto, existing);
                existing.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating inventory with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _logger.Info("Deleting inventory with ID {Id}.", id);
                var existing = await _context.Inventories.FindAsync(id);
                if (existing == null || !existing.Enable)
                    throw new KeyNotFoundException($"Inventory with ID {id} not found.");

                existing.Enable = false;
                existing.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting inventory with ID {Id}.", id);
                throw;
            }
        }
    }

}
