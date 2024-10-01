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
        IJobListingService jobListingService) : Controller
    {
        private readonly IJobApplicationService _jobApplicationService = jobApplicationService;
        private readonly IJobListingService _jobListingService = jobListingService;

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

            JobApplicationVM jobApplicationVM = new()
            {
                JobListing = jobListing
            };

            return View(jobApplicationVM);
        }
    }
}
