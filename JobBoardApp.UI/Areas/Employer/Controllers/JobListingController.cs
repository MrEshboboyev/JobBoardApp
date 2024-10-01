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
        
        public async Task<IActionResult> EmployerIndex()
        {
            var allJobListings = (await _jobListingService.GetEmployerJobListingsAsync(GetUserId())).Data;
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
                return RedirectToAction(nameof(EmployerIndex));
            }

            TempData["error"] = $"Failed to Job Listing creation process. Error : {result.Message}";
            return View(jobListingDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid jobListingId)
        {
            var jobListing = (await _jobListingService.GetJobListingAsync(jobListingId)).Data;

            return View(jobListing);
        }

        [HttpPost]
        public async Task<IActionResult> Update(JobListingDTO jobListingDTO)
        {
            jobListingDTO.EmployerId = GetUserId();

            var result = await _jobListingService.UpdateJobListingAsync(jobListingDTO);

            if (result.Success)
            {
                TempData["success"] = "Job listing updated successfully";
                return RedirectToAction(nameof(EmployerIndex));
            }

            TempData["error"] = $"Failed to Job Listing updated process. Error : {result.Message}";
            return View(jobListingDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid jobListingId)
        {
            JobListingDTO jobListingDTO = new()
            {
                Id = jobListingId,
                EmployerId = GetUserId()
            };

            var result = await _jobListingService.DeleteJobListingAsync(jobListingDTO);

            if (result.Success)
            {
                TempData["success"] = "Job Listing deleted successfully!";
                return RedirectToAction(nameof(EmployerIndex));
            }

            TempData["error"] = $"Failed to job listing delete process. Error : {result.Message}";
            return View(jobListingDTO);
        }
    }
}
