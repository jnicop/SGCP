using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGCP.DTOs.Security;
using SGCP.Models;
using SGCP.Security;
using SGCP.Services.Logger;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SGCP.Services;

public class AuthService : IAuthService
{
    private readonly SGCP_DbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly ILoggingService<AuthService> _logger;

    public AuthService(SGCP_DbContext context, IOptions<JwtSettings> jwtSettings, ILoggingService<AuthService> logger)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Enable);

        if (!user.Enable)
            throw new InvalidOperationException("User is disabled.");

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            _logger.Warn($"Login failed for user '{loginDto.Username}'");
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        // Guardar en BD
        var tokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsUsed = false,
            IsRevoked = false,
            InsertDate = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(tokenEntity);
        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Roles = user.UserRoles.Select(ur => ur.Role.Name)
        };
    }
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private bool VerifyPassword(string input, string hashedPassword)
    {
        // 🔐 Esto es un ejemplo. Reemplazar por hashing real como BCrypt o SHA256.
        return input == hashedPassword;
    }

    private string GenerateJwtToken(User user)
    {
        var roles = user.UserRoles.Select(ur => ur.Role).ToList();

        var permissions = roles
            .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission.Name))
            .Distinct()
            .ToList();

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
    };

        // Agregamos roles
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role.Name));

        // Agregamos permisos como claim personalizado
        claims.Add(new Claim("permissions", string.Join(",", permissions)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> RegisterAsync(RegisterUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            throw new InvalidOperationException("Username already exists.");

        if (dto.Password.Length < 6)
            throw new Exception("Password must be at least 6 characters long.");

        if (!string.IsNullOrWhiteSpace(dto.Email) && !new EmailAddressAttribute().IsValid(dto.Email))
            throw new Exception("Invalid email format.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Email = dto.Email,
            Enable = true,
            InsertDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Asignar roles
        if (dto.Roles != null && dto.Roles.Any())
        {
            var existingRoles = await _context.Roles
                .Where(r => dto.Roles.Contains(r.Name))
                .ToListAsync();

            foreach (var role in existingRoles)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }

            await _context.SaveChangesAsync();
        }

        _logger.Info($"User '{dto.Username}' registered with {dto.Roles?.Count ?? 0} role(s).");
        return true;
    }

    public async Task<bool> ChangePasswordAsync(long userId, ChangePasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new InvalidOperationException("Incorrect current password.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.UpdateDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.Info($"User {user.Username} changed password.");
        return true;
    }


    public async Task<UserInfoDto> GetUserInfoAsync(ClaimsPrincipal user)
    {
        var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr is null || !long.TryParse(userIdStr, out var userId))
            throw new UnauthorizedAccessException("Invalid token or missing user ID.");

        var entity = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId && u.Enable);

        if (entity is null)
            throw new Exception("User not found.");

        var dto = new UserInfoDto
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
            Roles = entity.UserRoles.Select(ur => ur.Role.Name).ToList(),
            Permissions = entity.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name))
                .Distinct()
                .ToList()
        };

        return dto;
    }
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var existingToken = await _context.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (existingToken == null || existingToken.IsUsed || existingToken.IsRevoked || existingToken.ExpirationDate < DateTime.UtcNow)
        {
            _logger.Warn("Invalid refresh token attempted.");
            throw new SecurityTokenException("Invalid or expired refresh token.");
        }

        // Marcar como usado
        existingToken.IsUsed = true;
        existingToken.UpdateDate = DateTime.UtcNow;

        // Generar nuevos tokens
        var user = existingToken.User;
        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        // Guardar nuevo refresh token
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            InsertDate = DateTime.UtcNow,
            IsUsed = false,
            IsRevoked = false
        });

        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            Username = user.Username,
            Roles = user.UserRoles.Select(ur => ur.Role.Name)
        };
    }
    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (token == null || token.IsRevoked || token.IsUsed)
            return false;

        token.IsRevoked = true;
        token.UpdateDate = DateTime.UtcNow;

        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> PurgeExpiredRefreshTokensAsync()
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(t =>
                t.ExpirationDate < DateTime.UtcNow || t.IsRevoked)
            .ToListAsync();

        if (!expiredTokens.Any())
            return 0;

        _context.RefreshTokens.RemoveRange(expiredTokens);
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> RevokeAllTokensAsync(long userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked)
            .ToListAsync();

        if (!tokens.Any())
            return false;

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.UpdateDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        _logger.Info($"All refresh tokens revoked for userId: {userId}");
        return true;
    }

}
