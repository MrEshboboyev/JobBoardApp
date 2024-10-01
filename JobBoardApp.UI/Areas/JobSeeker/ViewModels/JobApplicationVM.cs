using JobBoardApp.Application.DTOs;
using JobBoardApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.UI.Areas.JobSeeker.ViewModels
{
    public class JobApplicationVM
    {
        public Guid JobListingId { get; set; }
        public string JobSeekerId { get; set; }
        [Required]
        public string Resume { get; set; }
        [Required]
        public string CoverLetter { get; set; }
        [Required]
        public ApplicationStatus Status { get; set; }

        public JobListingDTO JobListing { get; set; }
    }
}
