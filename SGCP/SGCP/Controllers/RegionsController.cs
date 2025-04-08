using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Region;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;
        private readonly ILoggingService<RegionsController> _logger;

        public RegionsController(IRegionService regionService, ILoggingService<RegionsController> logger)
        {
            _regionService = regionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.Info("Requesting all regions.");
                var regions = await _regionService.GetAllAsync();
                return Ok(regions);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all regions.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                _logger.Info("Requesting region with ID {Id}.", id);
                var region = await _regionService.GetByIdAsync(id);
                return Ok(region);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error getting region by ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegionCreateDto dto)
        {
            try
            {
                _logger.Info("Creating new region.");
                var createdRegion = await _regionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdRegion.Id }, createdRegion);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating region.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] RegionUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating region with ID {Id}.", id);
                await _regionService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating region with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                _logger.Info("Deleting region with ID {Id}.", id);
                await _regionService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting region with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}