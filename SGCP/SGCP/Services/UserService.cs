using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.User;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class UserService : IUserService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<UserService> _logger;

        public UserService(SGCP_DbContext context, IMapper mapper, ILoggingService<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(long id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new InvalidOperationException("Username already exists.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Enable = true;
            user.InsertDate = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await AssignRolesAsync(user.Id, dto.Roles);

            _logger.Info($"User '{dto.Username}' created.");
            return await GetByIdAsync(user.Id) ?? throw new Exception("User creation failed.");
        }

        public async Task<bool> UpdateAsync(long id, UpdateUserDto dto)
        {
            var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.UpdateDate = DateTime.UtcNow;

            // Actualizar roles
            _context.UserRoles.RemoveRange(user.UserRoles);
            await _context.SaveChangesAsync();

            await AssignRolesAsync(user.Id, dto.Roles);

            _logger.Info($"User '{user.Username}' updated.");
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.Warn($"User '{user.Username}' deleted.");
            return true;
        }

        private async Task AssignRolesAsync(long userId, List<string> roleNames)
        {
            if (roleNames == null || !roleNames.Any()) return;

            var roles = await _context.Roles
                .Where(r => roleNames.Contains(r.Name))
                .ToListAsync();

            foreach (var role in roles)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = role.Id
                });
            }

            await _context.SaveChangesAsync();
        }
        public async Task<bool> SetUserStatusAsync(long userId, bool enable)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Enable = enable;
            user.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.Info($"User '{user.Username}' has been {(enable ? "enabled" : "disabled")}.");
            return true;
        }

    }
}
