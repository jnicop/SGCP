using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Unit;
using SGCP.Services;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet("unit-types")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _unitService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                return Ok(await _unitService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UnitCreateDto dto)
        {
            var createdUnit = await _unitService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUnit.Id }, createdUnit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UnitUpdateDto dto)
        {
            try
            {
                await _unitService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _unitService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }
    }
}
