namespace SGCP.DTOs.Security
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; } = null!;
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
