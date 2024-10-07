using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.UI.Areas.JobSeeker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize(Roles = "JobSeeker")]
    public class JobApplicationController(IJobApplicationService jobApplicationService,
        IJobListingService jobListingService,
        IUserProfileService userProfileService) : Controller
    {
        private readonly IJobApplicationService _jobApplicationService = jobApplicationService;
        private readonly IJobListingService _jobListingService = jobListingService;
        private readonly IUserProfileService _userProfileService = userProfileService;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required!");
        #endregion

        public async Task<IActionResult> Index()
        {
            var jobSeekerApplications = (await _jobApplicationService.GetJobSeekerApplicationsAsync(GetUserId())).Data;
            return View(jobSeekerApplications);
        }

        [HttpGet]
        public async Task<IActionResult> Apply(Guid jobListingId)
        {
            var jobListing = (await _jobListingService.GetJobListingAsync(jobListingId)).Data;
            var jobSeekerProfile = (await _userProfileService.GetProfileAsync(GetUserId())).Data;

            // Use the service to check if there is a pending or rejected application
            bool hasSubmittedApplication = (await _jobApplicationService.HasPendingOrRejectedApplication(jobListingId, GetUserId())).Data;

            var previousApplication = (await _jobApplicationService.GetPreviousApplication(jobListingId, GetUserId())).Data;

            JobApplicationVM jobApplicationVM = new()
            {
                JobListing = jobListing,
                ResumePath = jobSeekerProfile.ResumePath,
                HasSubmittedApplication = hasSubmittedApplication,
                ReapplyAfterDate = previousApplication?.ReapplyAfter
            };

            return View(jobApplicationVM);
        }


        [HttpPost]
        public async Task<IActionResult> Apply(JobApplicationVM jobApplicationVM)
        {
            JobApplicationDTO jobApplicationDTO = new()
            {
                JobSeekerId = GetUserId(),
                JobListingId = jobApplicationVM.JobListing.Id,
                ResumePath = jobApplicationVM.ResumePath,
                CoverLetter = jobApplicationVM.CoverLetter
            };

            var result = await _jobApplicationService.CreateJobApplicationAsync(jobApplicationDTO);

            if (result.Success)
            {
                TempData["success"] = "Job Application created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Failed to Job Application create process. Error : {result.Message}";
            return View(jobApplicationVM.JobListing.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid jobApplicationId)
        {
            JobApplicationDTO jobApplicationDTO = new()
            {
                Id = jobApplicationId,
                JobSeekerId = GetUserId(),
            };
            var result = await _jobApplicationService.GetJobSeekerApplicationAsync(jobApplicationDTO);

            if (!result.Success)
            {
                TempData["error"] = "Job Application not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }
    }
}
