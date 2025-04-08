using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Location;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly ILoggingService<LocationsController> _logger;

    public LocationsController(ILocationService locationService, ILoggingService<LocationsController> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.Info("Requesting all locations.");
            var locations = await _locationService.GetAllAsync();
            return Ok(locations);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting all locations.");
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            _logger.Info("Requesting location with ID {Id}.", id);
            var location = await _locationService.GetByIdAsync(id);
            return Ok(location);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.Warn(ex.Message);
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error getting location by ID {Id}.", id);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LocationCreateDto dto)
    {
        try
        {
            _logger.Info("Creating new location.");
            var createdLocation = await _locationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdLocation.Id }, createdLocation);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating location.");
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] LocationUpdateDto dto)
    {
        try
        {
            _logger.Info("Updating location with ID {Id}.", id);
            await _locationService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.Warn(ex.Message);
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating location with ID {Id}.", id);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            _logger.Info("Deleting location with ID {Id}.", id);
            await _locationService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.Warn(ex.Message);
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error deleting location with ID {Id}.", id);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }
}
