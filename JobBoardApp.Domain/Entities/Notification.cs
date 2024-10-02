namespace JobBoardApp.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relations
        public string RecipientId { get; set; }
        public AppUser Recipient { get; set; }

        public Guid? JobListingId { get; set; }
        public JobListing JobListing { get; set; }

        public Guid? JobApplicationId { get; set; }
        public JobApplication JobApplication { get; set; }
    }
}
