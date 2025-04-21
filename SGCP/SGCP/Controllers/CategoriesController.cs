using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Category;
using SGCP.Security.Authorization;
using SGCP.Services;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [HasPermission("Categories.Read")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _categoryService.GetAllAsync());

        [HttpGet("{id}")]
        [HasPermission("Categories.Read")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                return Ok(await _categoryService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }
        [HttpGet("category-type/{id}")]
        [HasPermission("Categories.Read")]
        public async Task<IActionResult> GetByType(int id)
        {
            try
            {
                return Ok(await _categoryService.GetByTypeAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }
        [HttpPost]
        [HasPermission("Categories.Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var createdCategory = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        [HasPermission("Categories.Update")]
        public async Task<IActionResult> Update(long id, [FromBody] CategoryUpdateDto dto)
        {
            try
            {
                await _categoryService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [HasPermission("Categories.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }
    }
}
