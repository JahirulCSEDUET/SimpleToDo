using AutoMapper;
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
        private readonly IToDoService _todoService;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly INotificationService _notificationService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ToDoController(IToDoService todoService, IUserService userService, INotificationService notificationService, IProjectService projectService, IFileService fileService, IMapper mapper)
        {
            _todoService = todoService;
            _userService = userService;
            _notificationService = notificationService;
            _projectService = projectService;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = GetCurrentUser();
            var todos = await _todoService.GetByUserIdWithProjectAsync(user.Id, false);
            var todoList = _mapper.Map<IReadOnlyList<ToDoItemListViewModel>>(todos);
            return View(todoList);
        }
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }
            return View(new ToDoItemCreateViewModel 
            { 
                ProjectId=projectId, 
                ProjectName=project.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ToDoItemCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = GetCurrentUser();

            var todo = _mapper.Map<Todo>(model);
            todo.CreatorId = user.Id;
            todo.CreatorName = user.FullName;
            
            try
            {
                if (model.File != null && model.File.Length > 0)
                {
                    using var stream = model.File.OpenReadStream();
                    var (storedPath, fileName) = await _fileService.SaveAsync(stream, model.File.FileName, "tasks");

                    todo.FilePath = storedPath;
                    todo.FileName = fileName;
                }
                await _todoService.AddAsync(todo);
                return RedirectToAction("Details", "Project", new { Id = model.ProjectId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.File), ex.Message);
                return View(model);
            }            
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
            var user = GetCurrentUser();
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
            var user = GetCurrentUser();
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
            if (todo == null)
            {
                TempData["AssignMemberError"] = $"Invalid todo.";
            }
            todo.UserId = userId;            
            await _todoService.UpdateAsync(todo);

            var currentUser = GetCurrentUser();
            var notification = new Notification
            {
                Title = "New Task Assigned",
                Message = $"{currentUser.FullName} assigned you to the task: {todo.Title} in workspace {todo.Project.Name}.",
                RedirectLink = RedirectLink.Todo,
                UserId = userId,
                IsRead = false,
                CreatedTime = DateTime.Now,
                RedirectId = todo.Id
            };
            await _notificationService.AddAsync(notification);
            return RedirectToAction("Details", "Project", new { Id = projectId });
        }
        public async Task<IActionResult> Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = GetCurrentUser();            
            var todo = await _todoService.GetByIdAsync(id);
            var todovm = _mapper.Map<ToDoItemDetailsViewModel>(todo);
            return View(todovm);
        }
        private User GetCurrentUser()
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
            return user;
        }
    }
}