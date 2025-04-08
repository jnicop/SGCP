using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Currency;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<CurrencyService> _logger;

        public CurrencyService(SGCP_DbContext context, IMapper mapper, ILoggingService<CurrencyService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllAsync()
        {
            _logger.Info("Getting all currencies.");
            var currencies = await _context.Currencies.Where(c => c.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<CurrencyDto>>(currencies);
        }

        public async Task<CurrencyDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting currency with ID {Id}.", id);
                var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == id && c.Enable == true);
                if (currency == null)
                {
                    _logger.Warn("Currency with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Currency with ID {id} not found.");
                }

                return _mapper.Map<CurrencyDto>(currency);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting currency with ID {Id}.", id);
                throw;
            }
        }

        public async Task<CurrencyDto> CreateAsync(CurrencyCreateDto dto)
        {
            try
            {
                _logger.Info("Creating a new currency.");
                var currency = _mapper.Map<Currency>(dto);
                currency.InsertDate = DateTime.UtcNow;
                currency.LastUpdate = DateTime.UtcNow;
                currency.Enable = true;

                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync();

                _logger.Info("Currency created with ID {Id}.", currency.Id);
                return _mapper.Map<CurrencyDto>(currency);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating new currency.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, CurrencyUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating currency with ID {Id}.", id);
                var existingCurrency = await _context.Currencies.FindAsync(id);
                if (existingCurrency == null)
                {
                    _logger.Warn("Currency with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Currency with ID {id} not found.");
                }

                _mapper.Map(dto, existingCurrency);
                existingCurrency.UpdateDate = DateTime.UtcNow;
                existingCurrency.LastUpdate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("Currency with ID {Id} updated successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating currency with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _logger.Info("Deleting currency with ID {Id}.", id);
                var existingCurrency = await _context.Currencies.FindAsync(id);
                if (existingCurrency == null || existingCurrency.Enable == false)
                {
                    _logger.Warn("Currency with ID {Id} not found or already deleted.", id);
                    throw new KeyNotFoundException($"Currency with ID {id} not found.");
                }

                existingCurrency.Enable = false;
                existingCurrency.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("Currency with ID {Id} deleted (logical) successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting currency with ID {Id}.", id);
                throw;
            }
        }
    }
}
