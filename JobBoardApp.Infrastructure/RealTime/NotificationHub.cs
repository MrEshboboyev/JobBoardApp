using JobBoardApp.Application.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace JobBoardApp.Infrastructure.RealTime
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationDTO(string userId, NotificationDTO notification)
        {
            await Clients.User(userId).SendAsync("ReceiveNotificationDTO", notification);
        }
    }
}
