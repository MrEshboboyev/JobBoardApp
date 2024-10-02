using AutoMapper;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Domain.Entities;

namespace JobBoardApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region UserProfile
            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap()
                    .ForMember(dest => dest.UserId, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore());
            #endregion

            #region JobListing
            CreateMap<JobListing, JobListingDTO>()
                .ForMember(dest => dest.EmployerName, opt => opt.MapFrom(src => src.Employer.UserName))
                .ReverseMap()
                    .ForMember(dest => dest.Employer, opt => opt.Ignore())
                    .ForMember(dest => dest.PostedDate, opt => opt.Ignore());
            #endregion

            #region JobApplication
            CreateMap<JobApplication, JobApplicationDTO>()
                .ForMember(dest => dest.JobSeekerName, opt => opt.MapFrom(src => src.JobSeeker.UserName))
                .ReverseMap()
                    .ForMember(dest => dest.JobSeeker, opt => opt.Ignore())
                    .ForMember(dest => dest.JobListing, opt => opt.Ignore())
                    .ForMember(dest => dest.ApplicationDate, opt => opt.Ignore());
            #endregion

            #region Notification
            CreateMap<Notification, NotificationDTO>()
                .ForMember(dest => dest.JobListingTitle, opt => opt.MapFrom(src => src.JobListing.Title))
                .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient.UserName))
                .ReverseMap()
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.JobApplication, opt => opt.Ignore())
                    .ForMember(dest => dest.JobListing, opt => opt.Ignore())
                    .ForMember(dest => dest.Recipient, opt => opt.Ignore());
            #endregion
        }
    }
}
