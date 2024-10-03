using JobBoardApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.Application.DTOs
{
    public class JobApplicationDTO
    {
        public Guid Id { get; set; }
        public Guid JobListingId { get; set; } 
        public string JobSeekerId { get; set; }
        [Required]
        public string ResumePath { get; set; }
        [Required]
        public string CoverLetter { get; set; }
        public DateTime ApplicationDate { get; set; }
        [Required]
        public ApplicationStatus Status { get; set; }

        public JobListingDTO JobListing { get; set; }
        public string JobSeekerName { get; set; }
    }
}
