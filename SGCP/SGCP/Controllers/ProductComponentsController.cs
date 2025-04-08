using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.ProductComponent;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductComponentsController : ControllerBase
    {
        private readonly IProductComponentService _service;
        private readonly ILoggingService<ProductComponentsController> _logger;

        public ProductComponentsController(IProductComponentService service, ILoggingService<ProductComponentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.Info("Getting all product components.");
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting product components.");
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
                _logger.Error(ex, "Error getting product component by ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductComponentCreateDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.ComponentId }, created);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating product component.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProductComponentUpdateDto dto)
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
                _logger.Error(ex, "Error updating product component with ID {Id}.", id);
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
                _logger.Error(ex, "Error deleting product component with ID {Id}.", id);
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}
