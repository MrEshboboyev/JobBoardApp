using JobBoardApp.Application.DTOs;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<ResponseDTO<IEnumerable<JobApplicationDTO>>> GetAllApplicationsAsync();
        Task<ResponseDTO<IEnumerable<JobApplicationDTO>>> GetJobSeekerApplicationsAsync(string jobSeekerId);
        Task<ResponseDTO<JobApplicationDTO>> GetJobSeekerApplicationAsync(JobApplicationDTO jobApplicationDTO);
        Task<ResponseDTO<JobApplicationDTO>> GetApplicationAsync(Guid applicationId);
        Task<ResponseDTO<bool>> CreateJobApplicationAsync(JobApplicationDTO jobApplicationDTO);
        Task<ResponseDTO<bool>> UpdateJobApplicationAsync(JobApplicationDTO jobApplicationDTO);
        Task<ResponseDTO<bool>> DeleteJobApplicationAsync(JobApplicationDTO jobApplicationDTO);

        // checking previous application
        Task<ResponseDTO<JobApplicationDTO>> GetPreviousApplication(Guid jobListingId, string jobSeekerId);
        Task<ResponseDTO<bool>> HasPendingOrRejectedApplication(Guid jobListingId, string jobSeekerId);
    }
}
