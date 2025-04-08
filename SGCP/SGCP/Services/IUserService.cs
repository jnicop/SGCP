using SGCP.DTOs.User;

namespace SGCP.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(long id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(long id, UpdateUserDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> SetUserStatusAsync(long userId, bool enable);

    }
}
