using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class JobListingController(IJobListingService jobListingService) : Controller
    {
        private readonly IJobListingService _jobListingService = jobListingService;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new Exception("Login required!");
        #endregion

        public async Task<IActionResult> Index()
        {
            var allJobListings = (await _jobListingService.GetAllJobListingsAsync()).Data;
            return View(allJobListings);
        }
    }
}
