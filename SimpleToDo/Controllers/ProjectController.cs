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
        private readonly IProjectMemberService _projectMemberService;
        public ProjectController(IProjectService projectService, IUserService userService, IProjectMemberService projectMemberService)
        {
            _projectService = projectService;
            _userService = userService;
            _projectMemberService = projectMemberService;
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
            var projectList = projects.Select(p=> new ProjectListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CurrentUserId = user.Id,
                ProjectMembers = p.ProjectMembers.Select(m => new ProjectMemberListViewModel
                {
                    Id = m.Id,
                    Role = m.Role,
                    UserId = m.UserId, 
                    UserName = m.User?.FullName ?? "N/A"
                }).ToList()
            }).ToList();
            return View(projectList);
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

            var project = await _projectService.GetByIdWithMemberAndTodoAsync(id);
            if (project == null) 
            { 
                return NotFound();
            }

            var projectMember = _projectMemberService.GetProjectMemberById(user.Id, project.Id);

            var projectvm = new ProjectDetailsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                DetailsViewerRole = projectMember.Role.ToString(),
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
                    CreatedBy = t.CreatorId,
                    CreatorName = t.CreatorName,
                    ProjectId = t.ProjectId,
                    Status =t.Status,
                    Title = t.Title
                }).ToList()
            };
            return View(projectvm);
        }
        [HttpPost]
        public async Task<IActionResult> QuickAddMember(int projectId, string email)
        {
            if (!ModelState.IsValid)
            {
                TempData["MemberError"] = $"Please enter email address.";
                return RedirectToAction("Details", new { id = projectId });
            }
            var user = _userService.GetByEmail(email);
            if (user == null)
            {
                TempData["MemberError"] = $"Could not find a user with the email '{email}'.";
                return RedirectToAction("Details", new { id = projectId });
            }
            var existingMember = _projectMemberService.GetProjectMemberById(user.Id, projectId);
            if (existingMember!=null)
            {
                TempData["MemberError"] = $"This user is already a member of the project.";
                return RedirectToAction("Details", new { id = projectId });
            }
            var projectMember = new ProjectMember
            {
                ProjectId = projectId,
                UserId = user.Id,
                Role = Role.Contributor
            };
            await _projectMemberService.AddAsync(projectMember);
            return RedirectToAction("Details","Project", new {Id=projectId});
        }
    }
}
