using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Permission;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<PermissionService> _logger;

        public PermissionService(SGCP_DbContext context, IMapper mapper, ILoggingService<PermissionService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PermissionDto>> GetAllAsync()
        {
            var permissions = await _context.Permissions.ToListAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}
