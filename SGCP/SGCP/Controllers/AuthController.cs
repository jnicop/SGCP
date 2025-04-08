using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SGCP.DTOs.Security;
using SGCP.Extensions;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILoggingService<AuthController> _logger;

    public AuthController(IAuthService authService, ILoggingService<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.Warn($"Login failed: {ex.Message}");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error while logging in user '{dto.Username}'.");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(new { message = "User created successfully." });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Warn(ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "User registration failed.");
            return StatusCode(500, new { message = "Internal server error." });
        }
    }

    //[Authorize]
    [HttpGet("userinfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var dto = await _authService.GetUserInfoAsync(User);
            return Ok(dto);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.Warn(ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving user info.");
            return StatusCode(500, new { message = "Internal server error." });
        }
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(result);
        }
        catch (SecurityTokenException ex)
        {
            _logger.Warn($"Refresh token failed: {ex.Message}");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error while refreshing token");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto dto)
    {
        var success = await _authService.LogoutAsync(dto.RefreshToken);

        if (!success)
            return BadRequest(new { message = "Invalid or already revoked refresh token." });

        return Ok(new { message = "Logged out successfully." });
    }

    [HttpPost("purge-refresh-tokens")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PurgeRefreshTokens()
    {
        var deletedCount = await _authService.PurgeExpiredRefreshTokensAsync();
        return Ok(new { message = $"Deleted {deletedCount} expired/revoked tokens." });
    }
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            var userId = User.GetUserId(); // extensión que ya tenemos
            var result = await _authService.ChangePasswordAsync(userId, dto);
            return result ? Ok(new { message = "Password changed successfully." }) : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            _logger.Warn(ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error changing password.");
            return StatusCode(500, new { message = "Internal server error." });
        }
    }
    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _authService.RevokeAllTokensAsync(userId);
            return result
                ? Ok(new { message = "All sessions closed." })
                : NotFound(new { message = "No active sessions found." });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during logout all.");
            return StatusCode(500, new { message = "Internal server error." });
        }
    }

}
