namespace SGCP.DTOs.User
{
    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
