using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Web.ViewModels.Auth;
using SimpleToDo.Web.ViewModels.ToDo;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IToDoService _todoService;

        public ToDoController(IToDoService todoService)
        {
            _todoService = todoService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var todos = await _todoService.GetByUserIdAsync(userId);
            var todoList = todos.Select(t => new ToDoItemListViewModel
            {
                Id = t.Id,
                Status = t.Status,
                Title = t.Title
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
            var todo = new ToDoItem
            {
                Status = Status.Pending,
                Title = item.Title,
                UserId = userId
            };
            await _todoService.AddAsync(todo);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _todoService.UpdateStatus(id, status);
            return View();
        }
    }
}
