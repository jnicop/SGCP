using SGCP.DTOs.Permission;

namespace SGCP.Services
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetAllAsync();
    }
}
