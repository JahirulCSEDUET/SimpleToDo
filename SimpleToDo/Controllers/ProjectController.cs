using AutoMapper;
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
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public ProjectController(IProjectService projectService, IUserService userService, IProjectMemberService projectMemberService, INotificationService notificationService, IMapper mapper)
        {
            _projectService = projectService;
            _userService = userService;
            _projectMemberService = projectMemberService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = GetCurrentUser();
            var projects = await _projectService.GetByMemberIdAsync(user.Id);
            var projectList = _mapper.Map<IReadOnlyList<ProjectListViewModel>>(projects);
            ViewBag.CurrentUserId = user.Id; 
            return View(projectList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProjectCreateViewModel model)
        {
            var user = GetCurrentUser();
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
            var user = GetCurrentUser();
            var projectMember = _projectMemberService.GetProjectMemberById(user.Id, id);
            if (projectMember == null)
            {
                return NotFound();
            }
            var project = await _projectService.GetByIdAsync(id);
            if(project == null)
            {
                return NotFound();
            }
            var projectvm = _mapper.Map<ProjectDetailsViewModel>(project);
            projectvm.DetailsViewerRole = projectMember.Role.ToString();
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

            var loginUser = GetCurrentUser();
            var project = await _projectService.GetByIdAsync(projectId);
            
            await _projectMemberService.AddAsync(projectMember);
            var notification = new Notification
            {
                Title = "Added to Workspace",
                Message = $"{loginUser.FullName} added you in workspace {project.Name}.",
                RedirectLink = RedirectLink.Project,
                UserId = user.Id,
                IsRead = false,
                CreatedTime = DateTime.Now,
                RedirectId = projectId
            };
            await _notificationService.AddAsync(notification);
            return RedirectToAction("Details","Project", new {Id=projectId});
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