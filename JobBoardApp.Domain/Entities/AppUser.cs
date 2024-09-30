using JobBoardApp.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoardApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        // No additional fields required for now, IdentityUser already includes Username, Email, etc.
        public DateTime DateRegistered { get; set; }

        // Navigation property to UserProfile
        public UserProfile UserProfile { get; set; }

        // Navigation property to job listings (if the user is an employer)
        public List<JobListing> JobListings { get; set; }

        // Navigation property to job applications (if the user is a job seeker)
        public List<JobApplication> JobApplications { get; set; }
    }
}
