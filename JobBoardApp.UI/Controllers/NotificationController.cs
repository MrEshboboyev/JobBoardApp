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
            var userNotifications = (await _notificationService.GetUserNotificationsAsync(GetUserId())).Data;
            return View(userNotifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(notificationId);

            if (result.Success)
            {
                TempData["success"] = "Mark notifications as Read success!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = $"Failed to mark notification as read process. Error : {result.Message}";
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
