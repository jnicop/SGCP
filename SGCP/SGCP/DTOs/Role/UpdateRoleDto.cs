namespace SGCP.DTOs.Role
{
    public class UpdateRoleDto
    {
        public string? Description { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
