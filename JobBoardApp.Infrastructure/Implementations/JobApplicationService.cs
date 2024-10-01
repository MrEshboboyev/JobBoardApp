﻿using AutoMapper;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Domain.Enums;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class JobApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : IJobApplicationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        
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
                var jobApplicationForDb = _mapper.Map<JobApplication>(jobApplicationDTO);
                jobApplicationForDb.ApplicationDate = DateTime.Now;
                jobApplicationForDb.Status = ApplicationStatus.Pending;

                await _unitOfWork.JobApplication.AddAsync(jobApplicationForDb);
                await _unitOfWork.SaveAsync();

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
                    ja => ja.JobSeekerId.Equals(jobApplicationDTO.JobSeekerId) &&
                    ja.Id.Equals(jobApplicationDTO.JobListingId)
                    ) ?? throw new Exception("Job Application not found!");

                // mapping fields
                _mapper.Map(jobApplicationDTO, jobApplicationFromDb);

                await _unitOfWork.JobApplication.UpdateAsync(jobApplicationFromDb);
                await _unitOfWork.SaveAsync();

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
