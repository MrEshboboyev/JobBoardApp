using JobBoardApp.Application.DTOs;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface IJobListingService
    {
        Task<ResponseDTO<IEnumerable<JobListingDTO>>> GetAllJobListingsAsync();
        Task<ResponseDTO<IEnumerable<JobListingDTO>>> GetEmployerJobListingsAsync(string employerId);
        Task<ResponseDTO<bool>> CreateJobListingAsync(JobListingDTO jobListingDTO);
        Task<ResponseDTO<bool>> UpdateJobListingAsync(JobListingDTO jobListingDTO);
        Task<ResponseDTO<bool>> DeleteJobListingAsync(JobListingDTO jobListingDTO);
    }
}
