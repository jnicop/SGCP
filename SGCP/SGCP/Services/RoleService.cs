using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Role;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class RoleService : IRoleService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<RoleService> _logger;

        public RoleService(SGCP_DbContext context, IMapper mapper, ILoggingService<RoleService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();

            return _mapper.Map<List<RoleDto>>(roles);
        }
        public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
        {
            if (await _context.Roles.AnyAsync(r => r.Name == dto.Name))
                throw new InvalidOperationException("Role already exists.");

            var role = _mapper.Map<Role>(dto);
            role.Enable = true;
            //role.InsertDate = DateTime.UtcNow;

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            await SetRolePermissions(role.Id, dto.Permissions);

            _logger.Info($"Role '{dto.Name}' created.");
            return await GetByIdAsync(role.Id) ?? throw new Exception("Creation failed.");
        }

        public async Task<bool> UpdateAsync(long id, UpdateRoleDto dto)
        {
            var role = await _context.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                role.Description = dto.Description;

            //role.UpdateDate = DateTime.UtcNow;

            // Quitar permisos existentes
            _context.RolePermissions.RemoveRange(role.RolePermissions);
            await _context.SaveChangesAsync();

            await SetRolePermissions(role.Id, dto.Permissions);

            _logger.Info($"Role '{role.Name}' updated.");
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            _logger.Warn($"Role '{role.Name}' deleted.");
            return true;
        }

        public async Task<RoleDto?> GetByIdAsync(long id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);

            return role == null ? null : _mapper.Map<RoleDto>(role);
        }

        private async Task SetRolePermissions(long roleId, List<string> permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any()) return;

            var permissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.Name))
                .ToListAsync();

            foreach (var perm in permissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = perm.Id
                });
            }

            await _context.SaveChangesAsync();
        }

    }
}
