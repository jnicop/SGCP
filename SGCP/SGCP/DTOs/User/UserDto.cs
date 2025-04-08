namespace SGCP.DTOs.User
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
