namespace SGCP.DTOs.Role
{
    public class CreateRoleDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
