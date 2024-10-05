using Microsoft.AspNetCore.Identity;

namespace JobBoardApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime DateRegistered { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

        public string? SuspensionReason { get; set; }
        public DateTime? SuspensionEndDate { get; set; }

        // Derived property
        public bool IsSuspended
        {
            get
            {
                return !string.IsNullOrEmpty(SuspensionReason) &&
                       (!SuspensionEndDate.HasValue || SuspensionEndDate > DateTime.Now);
            }
        }

        public UserProfile UserProfile { get; set; }
        public List<JobListing>? JobListings { get; set; }
        public List<JobApplication>? JobApplications { get; set; }
        public ICollection<Notification> Notifications { get; set; } = [];
    }
}
