using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.RegionalsPrice;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class RegionalsPriceService : IRegionalsPriceService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<RegionalsPriceService> _logger;

        public RegionalsPriceService(SGCP_DbContext context, IMapper mapper, ILoggingService<RegionalsPriceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RegionalsPriceDto>> GetAllAsync()
        {
            _logger.Info("Getting all regionals prices.");
            var items = await _context.RegionalsPrices.Where(x => x.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<RegionalsPriceDto>>(items);
        }

        public async Task<RegionalsPriceDto> GetByIdAsync(long id)
        {
            var item = await _context.RegionalsPrices.FirstOrDefaultAsync(x => x.Id == id && x.Enable == true);
            if (item == null)
            {
                _logger.Warn("RegionalsPrice with ID {Id} not found.", id);
                throw new KeyNotFoundException($"RegionalsPrice with ID {id} not found.");
            }
            return _mapper.Map<RegionalsPriceDto>(item);
        }

        public async Task<RegionalsPriceDto> CreateAsync(RegionalsPriceCreateDto dto)
        {
            var item = _mapper.Map<RegionalsPrice>(dto);
            item.InsertDate = DateTime.UtcNow;
            item.Enable = true;

            _context.RegionalsPrices.Add(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<RegionalsPriceDto>(item);
        }

        public async Task<bool> UpdateAsync(long id, RegionalsPriceUpdateDto dto)
        {
            var item = await _context.RegionalsPrices.FindAsync(id);
            if (item == null)
            {
                _logger.Warn("RegionalsPrice with ID {Id} not found.", id);
                throw new KeyNotFoundException($"RegionalsPrice with ID {id} not found.");
            }

            _mapper.Map(dto, item);
            item.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var item = await _context.RegionalsPrices.FindAsync(id);
            if (item == null || item.Enable == false)
            {
                _logger.Warn("RegionalsPrice with ID {Id} not found or already deleted.", id);
                throw new KeyNotFoundException($"RegionalsPrice with ID {id} not found.");
            }

            item.Enable = false;
            item.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
