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

            // Check if a new resume has been uploaded
            if (userProfileDTO.Resume != null)
            {
                var resumeFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/resumes");
                var uniqueResumeFileName = Guid.NewGuid().ToString() + "_" + userProfileDTO.Resume.FileName;
                var resumeFilePath = Path.Combine(resumeFolder, uniqueResumeFileName);

                if (!Directory.Exists(resumeFolder))
                    Directory.CreateDirectory(resumeFolder);

                using (var resumeStream = new FileStream(resumeFilePath, FileMode.Create))
                {
                    await userProfileDTO.Resume.CopyToAsync(resumeStream);
                }

                // Delete old resume if exists
                if (!string.IsNullOrEmpty(currentProfile.Data.ResumePath))
                {
                    var oldResumeFilePath = Path.Combine(_hostingEnvironment.WebRootPath, currentProfile.Data.ResumePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldResumeFilePath))
                        System.IO.File.Delete(oldResumeFilePath);
                }

                // Store the new resume path
                userProfileDTO.ResumePath = "/uploads/resumes/" + uniqueResumeFileName;
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
