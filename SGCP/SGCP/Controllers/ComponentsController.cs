using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Component;
using SGCP.Security.Authorization;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly ILoggingService<ComponentsController> _logger;

        public ComponentsController(IComponentService componentService, ILoggingService<ComponentsController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        [HttpGet]
        [HasPermission("Components.Read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.Info("Requesting all components.");
                var components = await _componentService.GetAllAsync();
                return Ok(components);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all components.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        [HasPermission("Components.Read")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                _logger.Info("Requesting component with ID {Id}.", id);
                var component = await _componentService.GetByIdAsync(id);
                return Ok(component);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error getting component by ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPost]
        [HasPermission("Components.Create")]
        public async Task<IActionResult> Create([FromBody] ComponentCreateDto dto)
        {
            try
            {
                _logger.Info("Creating new component.");
                var createdComponent = await _componentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdComponent.Id }, createdComponent);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating component.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        [HasPermission("Components.Update")]
        public async Task<IActionResult> Update(long id, [FromBody] ComponentUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating component with ID {Id}.", id);
                await _componentService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating component with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpDelete("{id}")]
        [HasPermission("Components.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                _logger.Info("Deleting component with ID {Id}.", id);
                await _componentService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting component with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }

}
