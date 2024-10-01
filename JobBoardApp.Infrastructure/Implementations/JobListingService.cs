using AutoMapper;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Entities;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class JobListingService(IUnitOfWork unitOfWork, IMapper mapper) : IJobListingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDTO<IEnumerable<JobListingDTO>>> GetAllJobListingsAsync()
        {
            try
            {
                var allJobListings = await _unitOfWork.JobListing.GetAllAsync(
                    includeProperties: "Employer");

                var mappedJobListings = _mapper.Map<IEnumerable<JobListingDTO>>(allJobListings);

                return new ResponseDTO<IEnumerable<JobListingDTO>>(mappedJobListings);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<JobListingDTO>>(ex.Message);
            }
        }

        public async Task<ResponseDTO<IEnumerable<JobListingDTO>>> GetEmployerJobListingsAsync(string employerId)
        {
            try
            {
                var employerJobListings = await _unitOfWork.JobListing.GetAllAsync(
                    filter: jl => jl.EmployerId.Equals(employerId),
                    includeProperties: "Employer,Applications");

                var mappedJobListings = _mapper.Map<IEnumerable<JobListingDTO>>(employerJobListings);

                return new ResponseDTO<IEnumerable<JobListingDTO>>(mappedJobListings);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<JobListingDTO>>(ex.Message);
            }
        }

        public async Task<ResponseDTO<JobListingDTO>> GetJobListingAsync(Guid jobListingId)
        {
            try
            {
                var jobListing = await _unitOfWork.JobListing.GetAsync(
                    filter: jl => jl.Id.Equals(jobListingId),
                    includeProperties: "Employer,Applications");

                var mappedJobListing = _mapper.Map<JobListingDTO>(jobListing);

                return new ResponseDTO<JobListingDTO>(mappedJobListing);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<JobListingDTO>(ex.Message);
            }
        }




        public async Task<ResponseDTO<bool>> CreateJobListingAsync(JobListingDTO jobListingDTO)
        {
            try
            {
                var jobListing = _mapper.Map<JobListing>(jobListingDTO);
                jobListing.PostedDate = DateTime.Now;

                await _unitOfWork.JobListing.AddAsync(jobListing);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UpdateJobListingAsync(JobListingDTO jobListingDTO)
        {
            try
            {
                // getting job listing
                var jobListingFromDb = await _unitOfWork.JobListing.GetAsync(
                    jl => jl.Id.Equals(jobListingDTO.Id) && 
                    jl.EmployerId.Equals(jobListingDTO.EmployerId)
                    ) ?? throw new Exception("Job Listing not found!");

                // update fields
                _mapper.Map(jobListingDTO, jobListingFromDb);

                await _unitOfWork.JobListing.UpdateAsync(jobListingFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> DeleteJobListingAsync(JobListingDTO jobListingDTO)
        {
            try
            {
                // getting job listing
                var jobListingFromDb = await _unitOfWork.JobListing.GetAsync(
                    jl => jl.Id.Equals(jobListingDTO.Id) &&
                    jl.EmployerId.Equals(jobListingDTO.EmployerId)
                    ) ?? throw new Exception("Job Listing not found!");

                await _unitOfWork.JobListing.RemoveAsync(jobListingFromDb);
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
