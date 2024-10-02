using AutoMapper;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Infrastructure.RealTime;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class NotificationService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IHubContext<NotificationHub> hubContext) : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task<ResponseDTO<IEnumerable<NotificationDTO>>> GetUserNotificationsAsync(string userId, bool includeRead = false)
        {
            try
            {
                var userNotifications = await _unitOfWork.Notification.GetAllAsync(
                    filter: n => n.RecipientId.Equals(userId),
                    includeProperties: "JobListing,Recipient");

                if (!includeRead)
                {
                    userNotifications = userNotifications.Where(n => !n.IsRead);
                }

                var mappedNotifications = _mapper.Map<IEnumerable<NotificationDTO>>(userNotifications);

                return new ResponseDTO<IEnumerable<NotificationDTO>>(mappedNotifications);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<NotificationDTO>>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> CreateNotificationAsync(NotificationDTO notificationDTO)
        {
            try
            {
                var notificationForDb = _mapper.Map<Notification>(notificationDTO);
                notificationForDb.CreatedAt = DateTime.Now;

                await _unitOfWork.Notification.AddAsync(notificationForDb);
                await _unitOfWork.SaveAsync();

                // sending notification
                await SendNotificationAsync(notificationDTO);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> SendNotificationAsync(NotificationDTO notificationDTO)
        {
            try
            {
                await _hubContext.Clients.User(notificationDTO.RecipientId).SendAsync("ReceiveNotification", notificationDTO.Message);
                
                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> MarkNotificationAsReadAsync(Guid notificationId)
        {
            try
            {
                var notificationFromDb = await _unitOfWork.Notification.GetAsync(
                    n => n.Id.Equals(notificationId)
                    ) ?? throw new Exception("Notification not found!");

                // update notification is read = true
                notificationFromDb.IsRead = true;

                await _unitOfWork.Notification.UpdateAsync(notificationFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> BroadcastNotificationToUserAsync(string recipientId, NotificationDTO notificationDTO)
        {
            try
            {
                await _hubContext.Clients.User(recipientId).SendAsync("ReceiveNotification", notificationDTO);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }
    }
}
