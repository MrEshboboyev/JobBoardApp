namespace JobBoardApp.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IJobApplicationRepository JobApplication { get; }
        IJobListingRepository JobListing { get; }
        IUserProfileRepository UserProfile { get; }
        INotificationRepository Notification { get; }

        Task SaveAsync();
    }
}
