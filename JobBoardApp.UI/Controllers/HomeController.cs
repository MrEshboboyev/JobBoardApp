using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobBoardApp.UI.Controllers
{
    public class HomeController(IJobListingService jobListingService) : Controller
    {
        private readonly IJobListingService _jobListingService = jobListingService;

        public async Task<IActionResult> Index()
        {
            var allJobListings = (await _jobListingService.GetAllJobListingsAsync()).Data;
            return View(allJobListings);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
