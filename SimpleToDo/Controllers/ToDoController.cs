using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Web.ViewModels.ToDo;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
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
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            await _todoService.UpdateStatus(id, status);
            return Ok();
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
    }
}
