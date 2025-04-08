using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.RegionalsPrice;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionalsPricesController : ControllerBase
    {
        private readonly IRegionalsPriceService _service;

        private readonly ILoggingService<RegionalsPricesController> _logger;

        public RegionalsPricesController(IRegionalsPriceService service, ILoggingService<RegionalsPricesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting regionals prices.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting regionals price with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegionalsPriceCreateDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating regionals price.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] RegionalsPriceUpdateDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating regionals price with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting regionals price with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}