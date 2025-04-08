using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Region;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class RegionService : IRegionService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<RegionService> _logger;

        public RegionService(SGCP_DbContext context, IMapper mapper, ILoggingService<RegionService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RegionDto>> GetAllAsync()
        {
            _logger.Info("Getting all regions.");
            var regions = await _context.Regions.Where(r => r.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<RegionDto>>(regions);
        }

        public async Task<RegionDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting region with ID {Id}.", id);
                var region = await _context.Regions.FirstOrDefaultAsync(r => r.Id == id && r.Enable == true);
                if (region == null)
                {
                    _logger.Warn("Region with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Region with ID {id} not found.");
                }

                return _mapper.Map<RegionDto>(region);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting region with ID {Id}.", id);
                throw;
            }
        }

        public async Task<RegionDto> CreateAsync(RegionCreateDto dto)
        {
            try
            {
                _logger.Info("Creating a new region.");
                var region = _mapper.Map<Region>(dto);
                region.InsertDate = DateTime.UtcNow;
                region.Enable = true;

                _context.Regions.Add(region);
                await _context.SaveChangesAsync();

                _logger.Info("Region created with ID {Id}.", region.Id);
                return _mapper.Map<RegionDto>(region);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating new region.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, RegionUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating region with ID {Id}.", id);
                var existingRegion = await _context.Regions.FindAsync(id);
                if (existingRegion == null)
                {
                    _logger.Warn("Region with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Region with ID {id} not found.");
                }

                _mapper.Map(dto, existingRegion);
                existingRegion.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("Region with ID {Id} updated successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating region with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _logger.Info("Deleting region with ID {Id}.", id);
                var existingRegion = await _context.Regions.FindAsync(id);
                if (existingRegion == null || existingRegion.Enable == false)
                {
                    _logger.Warn("Region with ID {Id} not found or already deleted.", id);
                    throw new KeyNotFoundException($"Region with ID {id} not found.");
                }

                existingRegion.Enable = false;
                existingRegion.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("Region with ID {Id} deleted (logical) successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting region with ID {Id}.", id);
                throw;
            }
        }
    }
}
