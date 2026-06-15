using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Features.ProjectMembers.Commands;
using SimpleToDo.Application.Features.ProjectMembers.Queries;
using SimpleToDo.Application.Features.Projects.Commands;
using SimpleToDo.Application.Features.Projects.Queries;
using SimpleToDo.Application.Features.Users.Queries;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Web.ViewModels.Project;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        // Clean constructor: Controller now only needs Mediator and AutoMapper
        public ProjectController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Challenge();

            var projects = await _mediator.Send(new GetProjectsByUserIdQuery(user.Id));
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user == null) return Challenge();

            await _mediator.Send(new CreateProjectCommand(model.Name, user.Id));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Challenge();

            // 3. Verify security access and roles via ProjectMembers queries
            var projectMember = await _mediator.Send(new GetProjectMemberByUserAndProjectIdQuery(user.Id, id));
            if (projectMember == null)
            {
                return NotFound();
            }

            // 4. Using GetProjectByIdQuery from your Queries folder structure
            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            if (project == null)
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
                TempData["MemberError"] = "Please enter email address.";
                return RedirectToAction("Details", new { id = projectId });
            }

            var loginUser = await GetCurrentUserAsync();
            if (loginUser == null) return Challenge();

            try
            {
                var command = new CreateProjectMemberCommand(projectId, email, loginUser.Id, loginUser.FullName);
                await _mediator.Send(command);
            }
            catch (InvalidOperationException ex)
            {
                
                TempData["MemberError"] = ex.Message;
            }

            return RedirectToAction("Details", "Project", new { Id = projectId });
        }


        private async Task<User?> GetCurrentUserAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;

            return await _mediator.Send(new GetUserByUserIdQuery(userId));
        }
    }
}