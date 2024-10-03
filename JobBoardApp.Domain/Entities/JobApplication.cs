using JobBoardApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoardApp.Domain.Entities
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public Guid JobListingId { get; set; } // Foreign key to JobListing
        public string JobSeekerId { get; set; } // Foreign key to User (Job Seeker)
        [Required]
        public string ResumePath { get; set; } // Could be a file path or the content
        [Required]
        public string CoverLetter { get; set; }
        public DateTime ApplicationDate { get; set; }
        [Required]
        public ApplicationStatus Status { get; set; }

        // Navigation properties
        public JobListing JobListing { get; set; }
        public AppUser JobSeeker { get; set; }
    }
}
