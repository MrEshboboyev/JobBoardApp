using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Controllers
{
    [Authorize]
    public class UserProfileController(IUserProfileService userProfileService,
        IWebHostEnvironment hostingEnvironment) : Controller
    {
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required;");
        #endregion

        public async Task<IActionResult> Index()
        {
            var userProfile = (await _userProfileService.GetProfileAsync(GetUserId())).Data;
            return View(userProfile);
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

            if (User.IsInRole("JobSeeker"))
            {
                var resumeFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/resumes");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + userProfileDTO.Resume.FileName;
                var resumePath = Path.Combine(resumeFolder, uniqueFileName);

                // Delete old resume
                var oldResumePath = userProfileDTO.ResumePath;
                if (Directory.Exists(oldResumePath))
                    Directory.Delete(oldResumePath);

                using (var resumeStream = new FileStream(resumePath, FileMode.Create))
                {
                    await userProfileDTO.Resume.CopyToAsync(resumeStream);
                }

                userProfileDTO.ResumePath = "/uploads/resumes/" + uniqueFileName;
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
