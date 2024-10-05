using JobBoardApp.Application.Common.Utility;
using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardApp.UI.Areas.Architect.Controllers
{
    [Area(SD.Role_Architect)]
    [Authorize(Roles = SD.Role_Architect)]
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allUsers = (await _userService.GetAllUsersAsync()).Data;
            return View(allUsers);
        }

        [HttpPost("Activate/{userName}")]
        public async Task<IActionResult> ActivateUser(string userName)
        {
            var result = (await _userService.ActivateUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User activated successfully." });
            return BadRequest(new { success = false, message = "User activation failed." });
        }

        [HttpPost("Deactivate/{userName}")]
        public async Task<IActionResult> DeactivateUser(string userName)
        {
            var result = (await _userService.DeactivateUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User deactivated successfully." });
            return BadRequest(new { success = false, message = "User deactivation failed." });
        }

        [HttpPost("Suspend")]
        public async Task<IActionResult> SuspendUser([FromBody] SuspendUserRequest request)
        {
            var result = (await _userService.SuspendUserAsync(request.UserId, 
                request.Reason, DateTime.Now)).Data;
            if (result)
                return Ok(new { success = true, message = "User suspended successfully." });
            return BadRequest(new { success = false, message = "User suspension failed." });
        }

        [HttpPost("Unlock/{userName}")]
        public async Task<IActionResult> UnlockUser(string userName)
        {
            var result = (await _userService.UnlockUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User unlocked successfully." });
            return BadRequest(new { success = false, message = "User unlocking failed." });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = (await _userService.ResetUserPasswordAsync(request.UserName, 
                request.NewPassword)).Data;
            if (result)
                return Ok(new { success = true, message = "Password reset successfully." });
            return BadRequest(new { success = false, message = "Password reset failed." });
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = (await _userService.AssignRoleAsync(request.UserName, request.Role)).Data;
            if (result)
                return Ok(new { success = true, message = "Role assigned successfully." });
            return BadRequest(new { success = false, message = "Role assignment failed." });
        }

        [HttpPost("Delete/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var result = (await _userService.DeleteUserAsync(userName)).Data;
            if (result)
                return Ok(new { success = true, message = "User deleted successfully." });
            return BadRequest(new { success = false, message = "User deletion failed." });
        }
    }

    // Request DTOs for various operations
    public class SuspendUserRequest
    {
        public string UserId { get; set; }
        public string Reason { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }

    public class AssignRoleRequest
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
