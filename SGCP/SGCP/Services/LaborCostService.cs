using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.LaborCost;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class LaborCostService : ILaborCostService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<LaborCostService> _logger;

        public LaborCostService(SGCP_DbContext context, IMapper mapper, ILoggingService<LaborCostService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LaborCostDto>> GetAllAsync()
        {
            _logger.Info("Getting all labor costs.");
            var items = await _context.LaborCosts.Where(x => x.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<LaborCostDto>>(items);
        }

        public async Task<LaborCostDto> GetByIdAsync(long id)
        {
            try
            {
                var item = await _context.LaborCosts.FirstOrDefaultAsync(x => x.Id == id && x.Enable == true);
                if (item == null)
                {
                    _logger.Warn("Labor cost with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Labor cost with ID {id} not found.");
                }
                return _mapper.Map<LaborCostDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting labor cost with ID {Id}.", id);
                throw;
            }
        }

        public async Task<LaborCostDto> CreateAsync(LaborCostCreateDto dto)
        {
            try
            {
                var item = _mapper.Map<LaborCost>(dto);
                item.InsertDate = DateTime.UtcNow;
                item.Enable = true;

                _context.LaborCosts.Add(item);
                await _context.SaveChangesAsync();

                return _mapper.Map<LaborCostDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating labor cost.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, LaborCostUpdateDto dto)
        {
            try
            {
                var item = await _context.LaborCosts.FindAsync(id);
                if (item == null)
                {
                    _logger.Warn("Labor cost with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Labor cost with ID {id} not found.");
                }

                _mapper.Map(dto, item);
                item.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating labor cost with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var item = await _context.LaborCosts.FindAsync(id);
                if (item == null || item.Enable == false)
                {
                    _logger.Warn("Labor cost with ID {Id} not found or already deleted.", id);
                    throw new KeyNotFoundException($"Labor cost with ID {id} not found.");
                }

                item.Enable = false;
                item.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting labor cost with ID {Id}.", id);
                throw;
            }
        }
    }

}
