using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Web.ViewModels.Project;
using SimpleToDo.Web.ViewModels.ToDo;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
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
            var projects = await _projectService.GetByMemberIdAsync(user.Id);

            return View(projects);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProjectCreateViewModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Challenge();
            }
            var user = _userService.GetByUserId(userId);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var project = new Project
            {
                Name = model.Name,
                ProjectMembers = new List<ProjectMember>()
                {
                    new ProjectMember
                    {
                        UserId =user.Id,
                        Role = Role.Admin
                    }
                }                
            };
            var projectCreated = await _projectService.AddAsync(project);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetByIdWithMemberAndTodoAsync(id);
            if (project == null) 
            { 
                return NotFound();
            }
            var projectvm = new ProjectDetailsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                ProjectMemberList = project.ProjectMembers.Select(pm => new ProjectMemberListViewModel
                {
                    Id = pm.Id,
                    UserName = pm.User.FullName,
                    Role = pm.Role,
                    UserId = pm.UserId
                }).ToList(),
                TodoList = project.Todos.Select(t=> new ToDoItemListViewModel
                {
                    Id =t.Id,
                    UserId = t.UserId??0,
                    UserName =t.User?.FullName??"Not Asigned.",
                    CreatedBy = t.CreatedBy,
                    Title = t.Title
                }).ToList()
            };
            return View(projectvm);
        }
        public IActionResult CreateTodo()
        {
            return View();
        }


    }
}
