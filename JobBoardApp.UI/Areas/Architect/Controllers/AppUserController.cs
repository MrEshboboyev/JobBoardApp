using JobBoardApp.Application.Common.Utility;
using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardApp.UI.Areas.Architect.Controllers
{
    [Area(SD.Role_Architect)]
    [Authorize(Roles = SD.Role_Architect)]
    public class AppUserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        public async Task<IActionResult> Index()
        {
            var allUsers = (await _userService.GetAllUsersAsync()).Data;
            return View(allUsers);
        }
    }
}
