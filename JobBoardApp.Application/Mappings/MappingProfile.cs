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
        }
    }
}
