using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using SGCP.DTOs.Catalogs;
using SGCP.DTOs.Category;
using SGCP.DTOs.Unit;
using SGCP.Models;

namespace SGCP.Services
{
    public class CatalogsService:ICatalogsService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;

        public CatalogsService(SGCP_DbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CatalogDto>> GetTreatmentTypesAsync()
        {
            var TreatmentTypes = await _context.TreatmentTypes.Where(c => c.Enable == true).OrderBy(t => t.Name).ToListAsync();
            return _mapper.Map<IEnumerable<CatalogDto>>(TreatmentTypes);
        }

        public async Task<IEnumerable<CatalogDto>> GetProcessTypesAsync()
        {
            var ProcessTypes = await _context.ProcessTypes.Where(c => c.Enable == true).OrderBy(t => t.Name).ToListAsync();
            return _mapper.Map<IEnumerable<CatalogDto>>(ProcessTypes);
        }

        public async Task<IEnumerable<CatalogDto>> GetScopeTypesAsync()
        {

            var ScopeTypes = await _context.ScopeTypes.Where(c => c.Enable == true).OrderBy(t => t.Name).ToListAsync();
            return _mapper.Map<IEnumerable<CatalogDto>>(ScopeTypes);

        }

        public async Task<IEnumerable<CatalogDto>> GetComponentTypesAsync()
        {
            var ComponentTypes = await _context.ComponentTypes.Where(c => c.Enable == true).OrderBy(t => t.Name).ToListAsync();
            return _mapper.Map<IEnumerable<CatalogDto>>(ComponentTypes);
        }
    }
}

