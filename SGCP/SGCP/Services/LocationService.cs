using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Location;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class LocationService : ILocationService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<LocationService> _logger;

        public LocationService(SGCP_DbContext context, IMapper mapper, ILoggingService<LocationService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LocationDto>> GetAllAsync()
        {
            _logger.Info("Getting all locations.");
            var locations = await _context.Locations.ToListAsync();
            return _mapper.Map<IEnumerable<LocationDto>>(locations);
        }

        public async Task<LocationDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting location with ID {Id}.", id);
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    _logger.Warn("Location with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Location with ID {id} not found.");
                }

                return _mapper.Map<LocationDto>(location);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting location with ID {Id}.", id);
                throw;
            }
        }

        public async Task<LocationDto> CreateAsync(LocationCreateDto dto)
        {
            try
            {
                _logger.Info("Creating a new location.");
                var location = _mapper.Map<Location>(dto);

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                _logger.Info("Location created with ID {Id}.", location.Id);
                return _mapper.Map<LocationDto>(location);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating new location.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, LocationUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating location with ID {Id}.", id);
                var existingLocation = await _context.Locations.FindAsync(id);
                if (existingLocation == null)
                {
                    _logger.Warn("Location with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Location with ID {id} not found.");
                }

                _mapper.Map(dto, existingLocation);
                await _context.SaveChangesAsync();

                _logger.Info("Location with ID {Id} updated successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating location with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _logger.Info("Deleting location with ID {Id}.", id);
                var existingLocation = await _context.Locations.FindAsync(id);
                if (existingLocation == null)
                {
                    _logger.Warn("Location with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Location with ID {id} not found.");
                }

                _context.Locations.Remove(existingLocation);
                await _context.SaveChangesAsync();

                _logger.Info("Location with ID {Id} deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting location with ID {Id}.", id);
                throw;
            }
        }
    }
}
