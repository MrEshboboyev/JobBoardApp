using JobBoardApp.Domain.Entities;

namespace JobBoardApp.Application.DTOs
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        public string RecipientId { get; set; }
        public string RecipientName { get; set; }

        public Guid? JobListingId { get; set; }
        public string JobListingTitle { get; set; }

        public Guid? JobApplicationId { get; set; }
    }
}
