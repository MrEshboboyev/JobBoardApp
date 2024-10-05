using JobBoardApp.Domain.Enums;

namespace JobBoardApp.Application.DTOs;

public class UserDTO
{
    public string Id { get; set; }                       // User ID
    public string UserName { get; set; }                 // Username
    public string Email { get; set; }                    // Email Address
    public IList<UserRole> Roles { get; set; }                   // User Role (e.g., JobSeeker, Employer, Admin)
    public bool IsActive { get; set; }                   // Is the user account active?
    public DateTime DateRegistered { get; set; }         // Date the user registered
    public DateTime? LastLoginDate { get; set; }         // Last login date (nullable)

    public string SuspensionReason { get; set; }         // Reason for suspension (if suspended)
    public DateTime? SuspensionEndDate { get; set; }     // End date for suspension (nullable)

    public DateTime? LockoutEnd { get; set; }            // Lockout end date (if the user is locked out)
}
