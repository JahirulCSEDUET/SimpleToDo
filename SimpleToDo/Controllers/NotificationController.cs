using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Application.Services;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Web.ViewModels.Notifications;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetFeed()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var user = _userService.GetByUserId(userId);
            if (user == null)
            {
                Challenge();
            }

            var noti = await _notificationService.GetByUserIdAsync(user.Id);
            var notification = noti.Select(n=> new NotificationListViewModel
            {
                Id = n.Id,
                IsRead = n.IsRead,
                Message = n.Message,
                RedirectLink = n.RedirectLink.ToString(),
                Title = n.Title,
                TimeAgo = (DateTime.Now-n.CreatedTime).Minutes,
                RedirectId = n.RedirectId
            }).ToList();
            ViewBag.UnreadCount = await _notificationService.CountUnreadNotificationByUserIdAsync(user.Id);
            return PartialView("_NotificationFeed", notification);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Challenge();

            var user = _userService.GetByUserId(userId);
            if (user == null) return Challenge();

            await _notificationService.MarkAsReadByUserIdAsync(user.Id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkSingleAsRead(int id)
        {
            await _notificationService.MarkAsReadByIdAsync(id);
            return Ok();
        }
    }
}
