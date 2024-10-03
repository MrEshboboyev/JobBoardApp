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

        [Display(Name = "Resume")]
        public IFormFile Resume { get; set; }

        public string? ResumePath { get; set; }
    }
}
