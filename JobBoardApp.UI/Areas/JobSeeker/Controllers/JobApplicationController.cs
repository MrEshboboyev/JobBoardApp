using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize(Roles = "JobSeeker")]
    public class JobApplicationController(IJobApplicationService jobApplicationService) : Controller
    {
        private readonly IJobApplicationService _jobApplicationService = jobApplicationService;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required!");
        #endregion

        public async Task<IActionResult> Index()
        {
            var jobSeekerApplications = (await _jobApplicationService.GetJobSeekerApplicationsAsync(GetUserId())).Data;
            return View(jobSeekerApplications);
        }
    }
}
