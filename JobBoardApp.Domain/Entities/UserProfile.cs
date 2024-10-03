namespace JobBoardApp.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Foreign key to User

        public string? CompanyName { get; set; } // Used for employers
        public string? Website { get; set; } // Used for employers
        public string? Bio { get; set; } // Used for job seekers, or both

        // New property for storing the profile picture path
        public string? ResumePath { get; set; }

        // Navigation property
        public AppUser User { get; set; }
    }
}
