using SGCP.DTOs.Security;
using System.Security.Claims;

namespace SGCP.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
        Task<bool> RegisterAsync(RegisterUserDto dto);
        Task<UserInfoDto> GetUserInfoAsync(ClaimsPrincipal user);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
        Task<int> PurgeExpiredRefreshTokensAsync();
        Task<bool> ChangePasswordAsync(long userId, ChangePasswordDto dto);
        Task<bool> RevokeAllTokensAsync(long userId);
    }
}
