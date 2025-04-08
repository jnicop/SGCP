using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs;
using SGCP.DTOs.Inventory;
using SGCP.Security.Authorization;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly IInventoryService _service;
    private readonly ILoggingService<InventoriesController> _logger;

    public InventoriesController(IInventoryService service, ILoggingService<InventoriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [HasPermission("Inventory.Read")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.Info("Getting all inventories.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting inventories.");
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpGet("{id}")]
    [HasPermission("Inventory.Read")]
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
            _logger.Error(ex, "Error getting inventory by ID {Id}.", id);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpPost]
    [HasPermission("Inventory.Create")]
    public async Task<IActionResult> Create([FromBody] InventoryCreateDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating inventory.");
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpPut("{id}")]
    [HasPermission("Inventory.Update")]
    public async Task<IActionResult> Update(long id, [FromBody] InventoryUpdateDto dto)
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
            _logger.Error(ex, "Error updating inventory with ID {Id}.", id);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    [HttpDelete("{id}")]
    [HasPermission("Inventory.Delete")]
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
            _logger.Error(ex, "Error deleting inventory with ID {Id}.", id);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }
}
