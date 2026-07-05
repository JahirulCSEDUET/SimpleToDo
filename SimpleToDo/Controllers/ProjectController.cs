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

            ViewBag.CurrentUserId = user.Id;
            return View(projects);
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

            var projectMember = await _mediator.Send(new GetProjectMemberByUserAndProjectIdQuery(user.Id, id));
            if (projectMember == null)
            {
                return NotFound();
            }

            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.ViewerRole = projectMember.role.ToString();

            return View(project);
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
                TempData["MemberSuccess"] = $"User: {email} added.";
            }
            catch (InvalidOperationException ex)
            {
                
                TempData["MemberError"] = ex.Message;
            }

            
            return RedirectToAction("Details", "Project", new { Id = projectId });
        }

        public async Task<IActionResult> Analytics(int id)
        {
            try
            {
                var report = await _mediator.Send(new GetProjectAnalyticsQuery(id));
                return View(report);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = GetCurrentUserAsync();
                var res = await _mediator.Send(new SafeDeleteProjectCommand(id, user.Id));
                if(res) 
                    TempData["DeleteSuccess"] = "Deleted Successfully";
                else
                    TempData["DeleteError"] = "Unsuccessfully";
            }
            catch (InvalidOperationException ex)
            {
                TempData["DeleteError"] = ex.Message;
            }
            catch(ArgumentNullException ex)
            {
                TempData["DeleteError"] = ex.Message;
            }
            return RedirectToAction("Details", "Project", new { Id = id });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateStatusAsync(int id, string status)
        {
            try
            {
                var user = GetCurrentUserAsync();
                await _mediator.Send(new UpdateProjectStatusCommand(id, status));
                TempData["UpdateSuccess"] = "Deleted Successfully";
            }
            catch (InvalidOperationException ex)
            {
                TempData["UpdateError"] = ex.Message;
            }
            catch (ArgumentNullException ex)
            {
                TempData["UpdateError"] = ex.Message;
            }
            return RedirectToAction("Details", "Project", new { Id = id });
        }
        private async Task<User?> GetCurrentUserAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;

            return await _mediator.Send(new GetUserByUserIdQuery(userId));
        }
    }
}