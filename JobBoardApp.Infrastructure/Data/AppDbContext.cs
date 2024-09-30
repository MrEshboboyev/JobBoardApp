﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobBoardApp.Domain.Entities;

namespace JobBoardApp.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : 
        IdentityDbContext<AppUser>(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important for IdentityDbContext

            // USER ENTITY
            modelBuilder.Entity<AppUser>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.UserProfile)    // One-to-One with UserProfile
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.JobListings)   // One-to-Many with JobListing
                .WithOne(jl => jl.Employer)
                .HasForeignKey(jl => jl.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.JobApplications) // One-to-Many with JobApplication
                .WithOne(ja => ja.JobSeeker)
                .HasForeignKey(ja => ja.JobSeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            // USERPROFILE ENTITY
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.Id);

            // JOBLISTING ENTITY
            modelBuilder.Entity<JobListing>()
                .HasKey(jl => jl.Id);

            modelBuilder.Entity<JobListing>()
                .HasMany(jl => jl.Applications)  // One-to-Many with JobApplication
                .WithOne(ja => ja.JobListing)
                .HasForeignKey(ja => ja.JobListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // JOBAPPLICATION ENTITY
            modelBuilder.Entity<JobApplication>()
                .HasKey(ja => ja.Id);

            // Timestamps using CURRENT_TIMESTAMP
            modelBuilder.Entity<AppUser>()
                .Property(u => u.DateRegistered)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<JobListing>()
                .Property(jl => jl.PostedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<JobApplication>()
                .Property(ja => ja.ApplicationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
