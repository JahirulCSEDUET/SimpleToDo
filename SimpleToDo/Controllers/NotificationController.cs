using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Features.Notifications.Commands;
using SimpleToDo.Application.Features.Notifications.Queries;
using SimpleToDo.Application.Features.Users.Queries;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ISender _mediotor;

        public NotificationController(ISender mediotor)
        {
            _mediotor = mediotor;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeed()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var user = await _mediotor.Send(new GetUserByUserIdQuery(userId));
            if (user == null)
            {
                Challenge();
            }

            var notification = await _mediotor.Send(new GetNotificationByUserIdQuery(user.Id));
            ViewBag.UnreadCount = await _mediotor.Send(new GetUnreadNotificationCountQuery(user.Id));
            return PartialView("_NotificationFeed", notification);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Challenge();

            var user = await _mediotor.Send( new GetUserByUserIdQuery(userId));
            if (user == null) return Challenge();

            await _mediotor.Send(new MarkAllNotificationReadCommand(user.Id));
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkSingleAsRead(int id)
        {
            await _mediotor.Send(new MarkNotifiicationAsReadByIdCommand(id));
            return Ok();
        }
    }
}
