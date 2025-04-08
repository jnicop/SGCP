using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Currency;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILoggingService<CurrenciesController> _logger;

        public CurrenciesController(ICurrencyService currencyService, ILoggingService<CurrenciesController> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.Info("Requesting all currencies.");
                var currencies = await _currencyService.GetAllAsync();
                return Ok(currencies);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all currencies.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                _logger.Info("Requesting currency with ID {Id}.", id);
                var currency = await _currencyService.GetByIdAsync(id);
                return Ok(currency);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error getting currency by ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurrencyCreateDto dto)
        {
            try
            {
                _logger.Info("Creating new currency.");
                var createdCurrency = await _currencyService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdCurrency.Id }, createdCurrency);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating currency.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] CurrencyUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating currency with ID {Id}.", id);
                await _currencyService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating currency with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                _logger.Info("Deleting currency with ID {Id}.", id);
                await _currencyService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting currency with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}