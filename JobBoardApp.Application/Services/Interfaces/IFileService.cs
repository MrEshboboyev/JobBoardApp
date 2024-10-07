using Microsoft.AspNetCore.Http;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadResumeAsync(IFormFile resume);
    }
}
