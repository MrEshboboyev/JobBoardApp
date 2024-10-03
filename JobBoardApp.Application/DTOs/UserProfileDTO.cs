using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.Application.DTOs
{
    public class UserProfileDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public string? CompanyName { get; set; }
        public string? Website { get; set; }
        public string? Bio { get; set; }

        public string? OwnerName { get; set; }

        // File upload for profile picture
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        public string? ProfilePicturePath { get; set; } // Stores the path of the uploaded profile picture
    }
}
