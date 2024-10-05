using JobBoardApp.Domain.Enums;

namespace JobBoardApp.Application.DTOs;

public class UserActivityDTO
{
    public string UserId { get; set; }                   // User ID
    public string UserName { get; set; }                 // Username
    public string Email { get; set; }                    // Email address
    public IList<UserRole> Roles { get; set; }                     // User role (e.g., JobSeeker, Employer, Admin)

    public IList<JobListingDTO> JobListings { get; set; }      // Number of job listings posted (for Employers)
    public IList<JobApplicationDTO> JobApplications { get; set; }  // Number of job applications submitted (for JobSeekers)

    public DateTime? LastLoginDate { get; set; }         // Last login date (nullable)
    public DateTime DateRegistered { get; set; }         // Date registered on the platform

    public bool IsActive { get; set; }                   // Is the user active or deactivated
}
