using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobApplicationController(IJobListingService jobListingService,
        IJobApplicationService jobApplicationService) : Controller
    {
        private readonly IJobListingService _jobListingService = jobListingService;
        private readonly IJobApplicationService _jobApplicationService = jobApplicationService;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required!");
        #endregion

        public async Task<IActionResult> ManageApplications()
        {
            var jobListings = (await _jobListingService.GetEmployerJobListingsAsync(GetUserId())).Data;
            return View(jobListings);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(Guid applicationId)
        {
            var application = (await _jobApplicationService.GetApplicationAsync(applicationId)).Data;
            return View(application);
        }
    }
}
