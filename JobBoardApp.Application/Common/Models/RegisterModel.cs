using JobBoardApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.Application.Common.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password), ErrorMessage = "Password and confirm password must be match.")]
        //public string ConfirmPassword { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public string? Bio { get; set; }
        public string? CompanyName { get; set; } // Used for employers
        public string? Website { get; set; } // Used for employers


        public IFormFile? Resume { get; set; }
        public string? ResumePath { get; set; }
    }
}
