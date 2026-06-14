using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class QueryController : Controller
    {
        private readonly IQueryService _queryService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;
        private readonly IToDoService _todoService;
        private readonly IMapper _mapper;
        public QueryController(IQueryService queryService, IUserService userService, IFileService fileService, INotificationService notificationService, IToDoService todoService, IMapper mapper)
        {
            _queryService = queryService;
            _userService = userService;
            _fileService = fileService;
            _notificationService = notificationService;
            _todoService = todoService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int todoId, string body, IFormFile? file=null)
        {
            if (!ModelState.IsValid)
            {
                TempData["QueryError"] = "Query message body content cannot be empty.";
                return RedirectToAction("Details", "Todo", new {Id= todoId});
            }
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
            var query = new Query
            {
                Body = body,
                TodoId = todoId,
                UserId = user.Id
            };
            
            try
            {
                if (file != null && file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var (storedPath, fileName) = await _fileService.SaveAsync(stream, file.FileName, "tasks");
                    query.FilePath = storedPath;
                    query.FileName = fileName;
                }
                await _queryService.AddAsync(query);
                var todo = await _todoService.GetByIdAsync(todoId);
                if(todo.UserId != user.Id)
                {
                    var notification = new Notification
                    {
                        Title = "New Query Posted",
                        Message = $"{user.FullName} has a query you to the task: {todo.Title} in workspace {todo.Project.Name}.",
                        RedirectLink = RedirectLink.Todo,
                        UserId = todo.UserId.Value,
                        IsRead = false,
                        CreatedTime = DateTime.Now,
                        RedirectId = todo.Id
                    };
                    await _notificationService.AddAsync(notification);
                }
                if(todo.CreatorId != user.Id)
                {
                    var notification = new Notification
                    {
                        Title = "New Query Posted",
                        Message = $"{user.FullName} posted a new query on the task '{todo.Title}' in workspace '{todo.Project.Name}'.",
                        RedirectLink = RedirectLink.Todo,
                        UserId = todo.CreatorId,
                        IsRead = false,
                        CreatedTime = DateTime.Now,
                        RedirectId = todo.Id
                    };
                    await _notificationService.AddAsync(notification);
                }
                
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
