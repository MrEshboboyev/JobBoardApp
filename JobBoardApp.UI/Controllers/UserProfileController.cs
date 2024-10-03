using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IO;

namespace JobBoardApp.UI.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserProfileController(IUserProfileService userProfileService, IWebHostEnvironment hostingEnvironment)
        {
            _userProfileService = userProfileService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required;");
        #endregion

        public async Task<IActionResult> Index()
        {
            var userProfile = (await _userProfileService.GetProfileAsync(GetUserId())).Data;

            UserProfileVM profileVM = new()
            {
                Bio = userProfile.Bio,
                Email = User.FindFirstValue(ClaimTypes.Email),
                UserName = userProfile.OwnerName,
                Website = userProfile.Website,
                CompanyName = userProfile.CompanyName,
                Id = userProfile.Id,
                OwnerName = userProfile.OwnerName,
                RoleName = User.FindFirstValue(ClaimTypes.Role),
                UserId = GetUserId(),
                ResumePath = userProfile.ResumePath
            };

            return View(profileVM);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var profile = await _userProfileService.GetProfileAsync(GetUserId());
            return View(profile.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserProfileDTO userProfileDTO)
        {
            if (!ModelState.IsValid)
                return View(userProfileDTO);

            var currentProfile = await _userProfileService.GetProfileAsync(GetUserId());

            // Update the user profile
            var result = await _userProfileService.UpdateProfileAsync(userProfileDTO);

            if (result.Success)
            {
                TempData["success"] = "Your profile has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Failed to update profile. Error: {result.Message}";
            return View(userProfileDTO);
        }

    }
}
