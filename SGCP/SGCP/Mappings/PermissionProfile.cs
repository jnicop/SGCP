using AutoMapper;
using SGCP.DTOs.Permission;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionDto>();
        }
    }
}
