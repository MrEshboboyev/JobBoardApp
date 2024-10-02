using JobBoardApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardApp.UI.Controllers
{
    [Authorize]
    public class NotificationController(INotificationService notificationService) : Controller
    {
        private readonly INotificationService _notificationService = notificationService;

        #region Private Methods
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Login required;");
        #endregion

        public async Task<IActionResult> Index()
        {
            var userNotifications = (await _notificationService.GetUserNotificationsAsync(GetUserId(), true)).Data;
            return View(userNotifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(id);

            if (result.Success)
            {
                TempData["success"] = "Notification marked as read successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Failed to mark notification as read. Error: {result.Message}";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> GetUnreadCount()
        {
            var unreadNotifications = (await _notificationService.GetUserNotificationsAsync(GetUserId(), false)).Data;

            var unreadCount = unreadNotifications.Count();
            return Ok(new { unreadCount });
        }
    }
}
