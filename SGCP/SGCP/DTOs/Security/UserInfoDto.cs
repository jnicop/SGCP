namespace SGCP.DTOs.Security
{
    public class UserInfoDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}
