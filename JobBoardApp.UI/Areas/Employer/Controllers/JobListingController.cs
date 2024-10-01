using JobBoardApp.Application.DTOs;
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

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(JobListingDTO jobListingDTO)
        {
            jobListingDTO.EmployerId = GetUserId();

            var result = await _jobListingService.CreateJobListingAsync(jobListingDTO);

            if (result.Success)
            {
                TempData["success"] = "Job Listing created successfully!";
                return RedirectToAction(nameof(UserIndex));
            }

            TempData["error"] = $"Failed to Job Listing creation process. Error : {result.Message}";
            return View(jobListingDTO);
        }
    }
}
