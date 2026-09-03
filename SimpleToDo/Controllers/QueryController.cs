using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleToDo.Application.Features.Queries.Commands;
using SimpleToDo.Application.Features.Users.Queries; 
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Web.Hubs;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class QueryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public QueryController(IMediator mediator, IFileService fileService, IHubContext<NotificationHub> hubContext)
        {
            _mediator = mediator;
            _fileService = fileService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int todoId, string body, IFormFile? file = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["QueryError"] = "Query message body content cannot be empty.";
                return RedirectToAction("Details", "Todo", new { Id = todoId });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Challenge(); 
            
            var user = await _mediator.Send(new GetUserByUserIdQuery(userId));
            if (user == null) return Challenge(); 

            string? storedPath = null;
            string? fileName = null;

            try
            {
                if (file != null && file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var (path, name) = await _fileService.SaveAsync(stream, file.FileName, "tasks");
                    storedPath = path;
                    fileName = name;
                }
                
                var command = new CreateQueryCommand(body, todoId, user.Id, storedPath, fileName);
                var Ids = await _mediator.Send(command);
                await _hubContext.Clients.Users(Ids).SendAsync("UpdateNotificationBadge");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(file), ex.Message);
                return RedirectToAction("Details", "Todo", new { Id = todoId });
            }

            return RedirectToAction("Details", "Todo", new { Id = todoId });
        }
    }
}