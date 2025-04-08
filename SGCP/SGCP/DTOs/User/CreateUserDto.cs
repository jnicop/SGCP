namespace SGCP.DTOs.User
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
