using JobBoardApp.Application.Common.Utility;
using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardApp.UI.Areas.Architect.Controllers
{
    [Area(SD.Role_Architect)]
    [Authorize(Roles = SD.Role_Architect)]
    public class JobApplicationController(IJobApplicationService jobApplicationService) : Controller
    {
        private readonly IJobApplicationService _jobApplicationService = jobApplicationService;

        public async Task<IActionResult> Index()
        {
            var allJobApplications = (await _jobApplicationService.GetAllApplicationsAsync()).Data;
            return View(allJobApplications);
        }
    }
}
