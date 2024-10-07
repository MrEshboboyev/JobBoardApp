using JobBoardApp.Application.DTOs;
using JobBoardApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardApp.UI.Areas.JobSeeker.ViewModels
{
    public class JobApplicationVM
    {
        [Required]
        public string ResumePath { get; set; }
        [Required]
        public string CoverLetter { get; set; }
        [Required]
        public ApplicationStatus Status { get; set; }

        public JobListingDTO JobListing { get; set; }

        // New properties for reapply restriction
        public bool HasSubmittedApplication { get; set; }
        public DateTime? ReapplyAfterDate { get; set; }
    }
}
