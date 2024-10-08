﻿using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Infrastructure.Data;

namespace JobBoardApp.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        private readonly AppDbContext _db = db;

        public IJobApplicationRepository JobApplication { get; private set; } = new JobApplicationRepository(db);
        public IJobListingRepository JobListing { get; private set; } = new JobListingRepository(db);
        public IUserProfileRepository UserProfile { get; private set; } = new UserProfileRepository(db);
        public INotificationRepository Notification { get; private set; } = new NotificationRepository(db);
        public IUserRepository User { get; private set; } = new UserRepository(db);

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
