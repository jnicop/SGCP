using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.InventoryMovement;
using SGCP.Security.Authorization;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [HasPermission("InventoryMovements.Read")]
    public class InventoryMovementsController : ControllerBase
    {
        private readonly IInventoryMovementService _service;
        private readonly ILoggingService<InventoryMovementsController> _logger;

        public InventoryMovementsController(IInventoryMovementService service, ILoggingService<InventoryMovementsController> logger)
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
                _logger.Error(ex, "Error getting all inventory movements.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting inventory movement {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [HasPermission("InventoryMovements.Create")]
        public async Task<IActionResult> Create([FromBody] InventoryMovementCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warn(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating inventory movement.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [HasPermission("InventoryMovements.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error deleting inventory movement {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
