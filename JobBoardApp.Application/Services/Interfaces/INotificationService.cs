using JobBoardApp.Application.DTOs;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseDTO<bool>> CreateNotificationAsync(NotificationDTO notificationDTO);
        Task<ResponseDTO<bool>> SendNotificationAsync(NotificationDTO notificationDTO);
        Task<ResponseDTO<IEnumerable<NotificationDTO>>> GetUserNotificationsAsync(string recipientId, bool includeRead = false);
        Task<ResponseDTO<bool>> MarkNotificationAsReadAsync(Guid notificationId);
        Task<ResponseDTO<bool>> BroadcastNotificationToUserAsync(string recipientId, NotificationDTO notificationDTO);  
    }
}
