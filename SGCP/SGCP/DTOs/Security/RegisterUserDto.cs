﻿namespace SGCP.DTOs.Security
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }

        public List<string>? Roles { get; set; } = new(); // opcional
    }
}
