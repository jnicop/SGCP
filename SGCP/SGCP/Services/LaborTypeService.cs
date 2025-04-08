using AutoMapper;
using SGCP.DTOs.LaborCost;
using SGCP.Models;
using SGCP.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace SGCP.Services
{
    public class LaborTypeService : ILaborTypeService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<LaborTypeService> _logger;

        public LaborTypeService(SGCP_DbContext context, IMapper mapper, ILoggingService<LaborTypeService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<LaborTypeDto>> GetAllAsync()
        {
            var types = await _context.LaborTypes
                .Where(t => t.Enable)
                .ToListAsync();

            return _mapper.Map<List<LaborTypeDto>>(types);
        }
    }
}
