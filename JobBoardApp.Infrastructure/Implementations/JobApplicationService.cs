using AutoMapper;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Domain.Enums;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class JobApplicationService(IUnitOfWork unitOfWork, IMapper mapper,
        INotificationService notificationService) : IJobApplicationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly INotificationService _notificationService = notificationService;

        public async Task<ResponseDTO<IEnumerable<JobApplicationDTO>>> GetAllApplicationsAsync()
        {
            try
            {
                var allApplications = await _unitOfWork.JobApplication.GetAllAsync(
                    includeProperties: "JobSeeker,JobListing.Employer"
                    );

                var mappedJobApplications = _mapper.Map<IEnumerable<JobApplicationDTO>>(allApplications);

                return new ResponseDTO<IEnumerable<JobApplicationDTO>>(mappedJobApplications);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<JobApplicationDTO>>(ex.Message);
            }
        }

        public async Task<ResponseDTO<IEnumerable<JobApplicationDTO>>> GetJobSeekerApplicationsAsync(string jobSeekerId)
        {
            try
            {
                var allJobsSeekerApplications = await _unitOfWork.JobApplication.GetAllAsync(
                    filter: ja => ja.JobSeekerId.Equals(jobSeekerId),
                    includeProperties: "JobSeeker,JobListing"
                    );

                var mappedJobApplications = _mapper.Map<IEnumerable<JobApplicationDTO>>(allJobsSeekerApplications);

                return new ResponseDTO<IEnumerable<JobApplicationDTO>>(mappedJobApplications);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<JobApplicationDTO>>(ex.Message);
            }
        }

        public async Task<ResponseDTO<JobApplicationDTO>> GetJobSeekerApplicationAsync(JobApplicationDTO jobApplicationDTO)
        {
            try
            {
                var jobSeekerApplication = await _unitOfWork.JobApplication.GetAsync(
                    filter: ja => ja.JobSeekerId.Equals(jobApplicationDTO.JobSeekerId) && 
                    ja.Id.Equals(jobApplicationDTO.Id),
                    includeProperties: "JobSeeker,JobListing,JobListing.Employer"
                    ) ?? throw new Exception("Job Application not found!");

                var mapperJobApplication = _mapper.Map<JobApplicationDTO>(jobSeekerApplication);

                return new ResponseDTO<JobApplicationDTO>(mapperJobApplication);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<JobApplicationDTO>(ex.Message);
            }
        }

        public async Task<ResponseDTO<JobApplicationDTO>> GetApplicationAsync(Guid applicationId)
        {
            try
            {
                var jobApplication = await _unitOfWork.JobApplication.GetAsync(
                    filter: ja => ja.Id.Equals(applicationId),
                    includeProperties: "JobSeeker,JobListing,JobListing.Employer"
                    ) ?? throw new Exception("Job Application not found!");

                var mappedJobApplication = _mapper.Map<JobApplicationDTO>(jobApplication);

                return new ResponseDTO<JobApplicationDTO>(mappedJobApplication);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<JobApplicationDTO>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> CreateJobApplicationAsync(JobApplicationDTO jobApplicationDTO)
        {
            try
            {
                var jobListingFromDb = await _unitOfWork.JobListing.GetAsync(
                    jl => jl.Id.Equals(jobApplicationDTO.JobListingId)
                    ) ?? throw new Exception("Job Listing not found!");

                var jobApplicationForDb = _mapper.Map<JobApplication>(jobApplicationDTO);
                jobApplicationForDb.ApplicationDate = DateTime.Now;
                jobApplicationForDb.Status = ApplicationStatus.Pending;

                await _unitOfWork.JobApplication.AddAsync(jobApplicationForDb);
                await _unitOfWork.SaveAsync();


                // create notification and send
                NotificationDTO notificationDTO = new()
                {
                    RecipientId = jobListingFromDb.EmployerId,
                    JobApplicationId = jobApplicationForDb.Id,
                    JobListingId = jobApplicationForDb.JobListingId,
                    Message = $"New job application received for your job listing: {jobListingFromDb.Title}"
                };

                await _notificationService.CreateNotificationAsync(notificationDTO);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UpdateJobApplicationAsync(JobApplicationDTO jobApplicationDTO)
        {
            try
            {
                var jobApplicationFromDb = await _unitOfWork.JobApplication.GetAsync(
                    filter: ja => ja.Id.Equals(jobApplicationDTO.Id),
                    includeProperties: "JobListing,JobSeeker"
                    ) ?? throw new Exception("Job Application not found!");

                jobApplicationFromDb.Status = jobApplicationDTO.Status;

                // if application rejected
                jobApplicationFromDb.ReapplyAfter = jobApplicationDTO.ReapplyAfter;

                await _unitOfWork.JobApplication.UpdateAsync(jobApplicationFromDb);
                await _unitOfWork.SaveAsync();

                // create notification and send
                NotificationDTO notificationDTO = new()
                {
                    RecipientId = jobApplicationFromDb.JobSeekerId,
                    JobListingId = jobApplicationFromDb.JobListingId,
                    JobApplicationId = jobApplicationFromDb.Id,
                    JobListingTitle = jobApplicationFromDb.JobListing.Title,
                    Message = $"Your application for the job listing " +
                    $"'{jobApplicationFromDb.JobListing.Title}'" +
                    $" has been updated to '{jobApplicationFromDb.Status}'"
                };

                await _notificationService.CreateNotificationAsync(notificationDTO);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> DeleteJobApplicationAsync(JobApplicationDTO jobApplicationDTO)
        {
            try
            {
                var jobApplicationFromDb = await _unitOfWork.JobApplication.GetAsync(
                    ja => ja.JobSeekerId.Equals(jobApplicationDTO.JobSeekerId) &&
                    ja.Id.Equals(jobApplicationDTO.JobListingId)
                    ) ?? throw new Exception("Job Application not found!");

                await _unitOfWork.JobApplication.RemoveAsync(jobApplicationFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }
    }
}
