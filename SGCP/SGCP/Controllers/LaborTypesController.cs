using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.Security.Authorization;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaborTypesController : ControllerBase
    {
        private readonly ILaborTypeService _service;
        private readonly ILoggingService<LaborTypesController> _logger;

        public LaborTypesController(ILaborTypeService service, ILoggingService<LaborTypesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [HasPermission("LaborTypes.Read")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener tipos de mano de obra.");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
