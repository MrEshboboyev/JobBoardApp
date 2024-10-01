using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobApplicationController(IJobListingService jobListingService) : Controller
    {
        private readonly IJobListingService _jobListingService = jobListingService;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required!");
        #endregion

        public async Task<IActionResult> ManageApplications()
        {
            var jobListings = (await _jobListingService.GetEmployerJobListingsAsync(GetUserId())).Data;
            return View(jobListings);
        }
    }
}
