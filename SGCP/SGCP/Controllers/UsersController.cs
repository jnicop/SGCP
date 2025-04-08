using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGCP.DTOs.User;
using SGCP.Security.Authorization;
using SGCP.Services;
using SGCP.Services.Logger;

namespace SGCP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggingService<UsersController> _logger;

        public UsersController(IUserService userService, ILoggingService<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [HasPermission("Users.Read")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [HasPermission("Users.Read")]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [HasPermission("Users.Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                var created = await _userService.CreateAsync(dto);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warn(ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating user.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [HasPermission("Users.Update")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var result = await _userService.UpdateAsync(id, dto);
                return result ? Ok(new { message = "User updated." }) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating user.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [HasPermission("Users.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _userService.DeleteAsync(id);
            return result ? Ok(new { message = "User deleted." }) : NotFound();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        [HasPermission("Users.Update")]
        public async Task<IActionResult> ToggleStatus(long id, [FromBody] ToggleUserStatusDto dto)
        {
            try
            {
                var result = await _userService.SetUserStatusAsync(id, dto.Enable);
                return result
                    ? Ok(new { message = $"User {(dto.Enable ? "enabled" : "disabled")} successfully." })
                    : NotFound(new { message = "User not found." });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error toggling user status.");
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

    }
}
