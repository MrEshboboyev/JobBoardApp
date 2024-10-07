using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Requests;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Enums;
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

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid applicationId, ApplicationStatus status)
        {
            JobApplicationDTO jobApplicationDTO = new()
            {
                Id = applicationId,
                Status = status
            };

            var result = await _jobApplicationService.UpdateJobApplicationAsync(jobApplicationDTO);

            if (result.Success)
            {
                TempData["success"] = "Applications Status updated!";
                return RedirectToAction(nameof(ManageApplications));
            }

            TempData["error"] = $"Failed to Application update status process. Error : {result.Message}";
            return View(jobApplicationDTO.Id);
        }

        [HttpPost("RejectApplication")]
        public async Task<IActionResult> RejectApplication([FromBody] RejectApplicationRequest request)
        {
            JobApplicationDTO jobApplicationDTO = new()
            {
                Id = request.ApplicationId,
                ReapplyAfter = request.ReapplyDate,
                Status = ApplicationStatus.Rejected
            };

            var result = await _jobApplicationService.UpdateJobApplicationAsync(jobApplicationDTO);

            if (result.Success)
            {
                return Ok(new { success = true, message = $"Applications is rejected! Reapply After : {request.ReapplyDate}" });
            }

            return BadRequest(new { success = false, message = result.Message });
        }
    }
}
