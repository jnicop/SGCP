using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs;
using SGCP.DTOs.Product;
using SGCP.Models;
using SGCP.Security.Authorization;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILoggingService<ProductsController> _logger;

        public ProductsController(IProductService productService, ILoggingService<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        //[HttpGet("activos")]
        //[HttpGet]
        //[HasPermission("Products.Read")]
        //public async Task<IActionResult> GetActivos()
        //{
        //    _logger.Info("Obteniendo productos activos...");
        //    try
        //    {
        //        var products = await _productService.GetProductsAsync();

        //        _logger.Info("Se obtuvieron {Count} productos activos.", products.Count());

        //        return Ok(products);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, "Error obteniendo productos activos");
        //        return StatusCode(500, "Error interno del servidor");
        //    }
        //}
        [HttpPost("paged")]
        [HasPermission("Products.Read")]
        public async Task<IActionResult> GetPaged([FromBody] PaginationQueryDto query)
        {
            _logger.Info("Obteniendo productos paginados: página {Page}, tamaño {Size}", query.PageIndex, query.PageSize);
            try
            {
                var result = await _productService.GetPagedProductsAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener productos paginados.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        [HasPermission("Products.Read")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
        }

        [HttpPost]
        [HasPermission("Products.Create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto product)
        {
            var createdProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        [HasPermission("Products.Update")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto product)
        {
            try
            {
                await _productService.UpdateAsync(id, product);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [HasPermission("Products.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _productService.DeleteAsync(id);
                return Ok(new { success = result, message = "Producto eliminado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.Warn(ex.Message);
                return NotFound(new { ex.Message });
            }
        }

        [HttpGet("builder/{id}")]
        [HasPermission("Products.Read")]
        public async Task<IActionResult> GetBuilder(long id)
        {
            try
            {
                var result = await _productService.GetBuilderAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error obteniendo producto para edición");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("builder")]
        [HasPermission("Products.Create")]
        //[HasPermission("Products.Update")]
        public async Task<IActionResult> SaveBuilder([FromBody] ProductBuilderDto dto)
        {
            try
            {
                var result = await _productService.SaveBuilderAsync(dto);

                return Ok(new
                {
                    message = dto.ProductId == 0
                        ? "Producto creado correctamente"
                        : "Producto actualizado correctamente",
                    product = result
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al guardar el producto");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}/enable")]
        [HasPermission("Products.Delete")]
        public async Task<IActionResult> SetEnable(long id, [FromQuery] bool enable)
        {
            var result = await _productService.SetEnableStatusAsync(id, enable);
            return Ok(new { success = result, enabled = enable });
        }

        [HttpPost("products/{productId}/recalculate-price")]
        [HasPermission("Products.Create")]
        public async Task<ActionResult<ProductPriceDto>> RecalculatePrice(long productId)
        {
            var result = await _productService.RecalculatePricesAsync(productId);
            return Ok(result);
        }
    }

}