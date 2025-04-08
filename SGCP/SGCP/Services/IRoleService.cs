using SGCP.DTOs.Role;

namespace SGCP.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto> CreateAsync(CreateRoleDto dto);
        Task<bool> UpdateAsync(long id, UpdateRoleDto dto);
        Task<bool> DeleteAsync(long id);
        Task<RoleDto?> GetByIdAsync(long id);
    }
}
