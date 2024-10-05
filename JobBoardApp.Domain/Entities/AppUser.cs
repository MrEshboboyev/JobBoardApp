using Microsoft.AspNetCore.Identity;

namespace JobBoardApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        // No additional fields required for now, IdentityUser already includes Username, Email, etc.
        public DateTime DateRegistered { get; set; }

        // Nullable field to track the last login date
        public DateTime? LastLoginDate { get; set; }

        // Field to indicate if the account is active or deactivated
        public bool IsActive { get; set; } = true;

        // Suspension information (reason for suspension, suspension end date)
        public string SuspensionReason { get; set; }
        public DateTime? SuspensionEndDate { get; set; }

        // Navigation property to UserProfile
        public UserProfile UserProfile { get; set; }

        // Navigation property to job listings (if the user is an employer)
        public List<JobListing> JobListings { get; set; }

        // Navigation property to job applications (if the user is a job seeker)
        public List<JobApplication> JobApplications { get; set; }

        public ICollection<Notification> Notifications { get; set; } = [];
    }
}
