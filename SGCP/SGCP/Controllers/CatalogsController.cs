using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.Catalogs;
using SGCP.Services;

namespace SGCP.Controllers
{
    [ApiController]
    [Route("api/catalogs")]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogsService _service;

        public CatalogsController(ICatalogsService service)
        {
            _service = service;
        }

        [HttpGet("treatment-types")]
        public async Task<ActionResult<List<CatalogDto>>> GetTreatmentTypes()
        {
            return Ok(await _service.GetTreatmentTypesAsync());
        }

        [HttpGet("process-types")]
        public async Task<ActionResult<List<CatalogDto>>> GetProcessTypes()
        {
            return Ok(await _service.GetProcessTypesAsync());
        }

        [HttpGet("scope-types")]
        public async Task<ActionResult<List<CatalogDto>>> GetScopeTypes()
        {
            return Ok(await _service.GetScopeTypesAsync());
        }
        [HttpGet("component-types")]
        public async Task<ActionResult<List<CatalogDto>>> GetComponentTypes()
        {
            return Ok(await _service.GetComponentTypesAsync());
        }

    }
}
