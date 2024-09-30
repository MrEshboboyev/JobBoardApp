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
                .ReverseMap();
            #endregion
        }
    }
}
