using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Web.ViewModels.Queries;
using SimpleToDo.Web.ViewModels.ToDo;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IToDoService _todoService;
        private readonly IUserService _userService;

        public ToDoController(IToDoService todoService, IUserService userService)
        {
            _todoService = todoService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var user =_userService.GetByUserId(userId);
            if (user == null)
            {
                Challenge();
            }
            var todos = await _todoService.GetByUserIdWithProjectAsync(user.Id, false);
            var todoList = todos.Select(t => new ToDoItemListViewModel
            {
                Id = t.Id,
                Status = t.Status,
                Title = t.Title,
                ProjectId = t.ProjectId?? 0,
                ProjectName = t.Project?.Name??"N/A",
                CreatorName =t.CreatorName
            }).ToList();
            return View(todoList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ToDoItemCreateViewModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
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
            var todo = new Todo
            {
                Status = Status.Pending,
                Title = item.Title,
                UserId = user.Id,
                CreatorId = user.Id,
                CreatorName =user.FullName,
                IsArchived = false,
            };
            await _todoService.AddAsync(todo);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> QuickCreate(int projectId, string title)
        {
            if (!ModelState.IsValid)
            {
                return View("Details","Project");
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
            var todo = new Todo
            {
                Status = Status.Pending,
                Title = title,
                ProjectId = projectId,
                CreatorId = user.Id,
                CreatorName=user.FullName,
                IsArchived = false,
            };
            await _todoService.AddAsync(todo);
            return RedirectToAction("Details","Project", new { id = projectId });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status,int? userId=null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data model state discovered." });
            }
            var todo = await _todoService.GetByIdAsync(id); 
            if(todo == null)
            {
                return NotFound(new { message = "The requested task could not be found." });
            }
            if (!userId.HasValue || userId==0)
            {
                return BadRequest(new { message = "Status update failed: To-do is unassigned." });
            }
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var user = _userService.GetByUserId(userID);
            if (user == null)
            {
                Challenge();
            }
            if(user.Id != userId)
            {
                return BadRequest(new { message = "Status update failed: You are not assigned to this to-do." });
            }
            await _todoService.UpdateStatus(id, status);
            return Ok(new { success = true, message = "Status updated successfully." });
        }
        [HttpPost]
        public async Task<IActionResult> ArchivedUnArchived(int id)
        {
            bool result = await _todoService.ArchiveUnarchivedAsync(id);
            if(!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ArchivedList()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var user = _userService.GetByUserId(userId);
            var todos = await _todoService.GetByUserIdAsync(user.Id, true);
            var todoList = todos.Select(t => new ToDoItemListViewModel
            {
                Id = t.Id,
                Status = t.Status,
                Title = t.Title
            }).ToList();
            return View(todoList);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            bool result = await _todoService.DeleteAsync(id);
            return RedirectToAction(nameof(ArchivedList));
        }
        [HttpPost]
        public async Task<IActionResult> QuickAssign(int userId, int projectId, int todoId)
        {
            if (!ModelState.IsValid)
            {
                TempData["AssignMemberError"] = $"Assigmn member unsuccessfull.";
                return RedirectToAction("Details", "Project", new { id = projectId });
            }
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) 
            {
                TempData["AssignMemberError"] = $"Invalid member.";
            }
            var todo = await _todoService.GetByIdAsync(todoId);
            if (user == null)
            {
                TempData["AssignMemberError"] = $"Invalid todo.";
            }
            todo.UserId = userId;
            await _todoService.UpdateAsync(todo);
            return RedirectToAction("Details", "Project", new { Id = projectId });
        }
        public async Task<IActionResult> Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
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
            var todo = await _todoService.GetByIdAsync(id);
            var todovm = new ToDoItemDetailsViewModel
            {
                Id = todo.Id,
                Title = todo.Title,
                Status = todo.Status,
                CreatorId = todo.CreatorId,
                CreatorName = todo.CreatorName,
                UserId = todo.UserId.Value,
                UserName = todo.User.FullName,
                ProjectId = todo.ProjectId.Value,
                ProjectName = todo.Project.Name,
                QueryList = todo.Queries.Select(q => new QueryListViewModel
                {
                    Id = q.Id,
                    Body = q.Body,
                    UserId = q.UserId,
                    UserName = q.User.FullName
                }).ToList()
            };
            return View(todovm);
        }
    }
}
