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
                UserId = GetUserId()
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

            // Check if a file has been uploaded
            if (userProfileDTO.ProfilePicture != null)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/profile_pictures");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + userProfileDTO.ProfilePicture.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await userProfileDTO.ProfilePicture.CopyToAsync(fileStream);
                }

                // Store the file path in the user's profile
                userProfileDTO.ProfilePicturePath = "/uploads/profile_pictures/" + uniqueFileName;
            }

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
