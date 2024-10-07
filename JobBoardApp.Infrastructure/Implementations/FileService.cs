using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadResumeAsync(IFormFile resume)
        {
            if (resume == null)
                return null;

            var resumeFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/resumes");
            var uniqueResumeFileName = Guid.NewGuid().ToString() + "_" + resume.FileName;
            var resumeFilePath = Path.Combine(resumeFolder, uniqueResumeFileName);

            if (!Directory.Exists(resumeFolder))
                Directory.CreateDirectory(resumeFolder);

            using (var resumeStream = new FileStream(resumeFilePath, FileMode.Create))
            {
                await resume.CopyToAsync(resumeStream);
            }

            return "/uploads/resumes/" + uniqueResumeFileName;
        }
    }
}
