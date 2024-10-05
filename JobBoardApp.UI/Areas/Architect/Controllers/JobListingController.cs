using JobBoardApp.Application.Common.Utility;
using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardApp.UI.Areas.Architect.Controllers
{
    [Area(SD.Role_Architect)]
    [Authorize(Roles = SD.Role_Architect)]
    public class JobListingController(IJobListingService jobListingService) : Controller
    {
        private readonly IJobListingService _jobListingService = jobListingService;

        public async Task<IActionResult> Index()
        {
            var allJobListings = (await _jobListingService.GetAllJobListingsAsync()).Data;
            return View(allJobListings);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid jobListingId)
        {
            var jobListing = (await _jobListingService.GetJobListingAsync(jobListingId)).Data;
            return View(jobListing);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid jobListingId)
        {
            var jobListing = (await _jobListingService.GetJobListingAsync(jobListingId)).Data;
            return View(jobListing);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(Guid jobListingId)
        {
            var result = await _jobListingService.DeleteJobListingAsyncByAdmin(jobListingId);
            
            if (result.Success)
            {
                TempData["success"] = "Job Listing updated";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Failed to Job Listing Update process. Error : {result.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
