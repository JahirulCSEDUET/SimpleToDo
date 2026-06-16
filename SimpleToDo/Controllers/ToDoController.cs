using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Application.Features.Projects.Queries;
using SimpleToDo.Application.Features.Todos.Commands;
using SimpleToDo.Application.Features.Todos.Queries;
using SimpleToDo.Application.Features.Users.Queries;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Web.ViewModels.ToDo;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public ToDoController(IMediator mediator, IFileService fileService, IMapper mapper)
        {
            _mediator = mediator;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Challenge();

            var todos = await _mediator.Send(new GetTodoByUserIdQuery(user.Id, false));

            return View(todos);
        }

        public async Task<IActionResult> Create(int projectId)
        {

            var project = await _mediator.Send(new GetProjectByIdQuery(projectId));
            if (project == null) return NotFound();

            return View(new ToDoItemCreateViewModel { ProjectId =projectId, ProjectName = project.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ToDoItemCreateViewModel model)
        {

            if (!ModelState.IsValid) {
                var project = await _mediator.Send(new GetProjectByIdQuery(model.ProjectId));
                if (project == null) return NotFound();

                ViewBag.ProjectId = project.Id;
                ViewBag.ProjectName = project.Name;
                return View(model); 
            }

            var user = await GetCurrentUserAsync();
            if (user == null) return Challenge();

            string? storedPath = null;
            string? fileName = null;

            try
            {
                
                if (model.File != null && model.File.Length > 0)
                {
                    using var stream = model.File.OpenReadStream();
                    var (path, name) = await _fileService.SaveAsync(stream, model.File.FileName, "tasks");
                    storedPath = path;
                    fileName = name;
                }

                var command = new CreateTodoCommand(
                    Title: model.Title,
                    ProjectId: model.ProjectId,
                    Description: model.Description,
                    CreatorId: user.Id,
                    CreatorName: user.FullName,
                    FilePath: storedPath,
                    FileName: fileName
                );

                await _mediator.Send(command);
                return RedirectToAction("Details", "Project", new { Id = model.ProjectId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(File), ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status, int? userId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data model state discovered." });
            }

            try
            {
                var todoDto = await _mediator.Send(new GetTodoByIdQuery(id));

                if (!userId.HasValue || userId == 0)
                {
                    return BadRequest(new { message = "Status update failed: To-do is unassigned." });
                }

                var user = await GetCurrentUserAsync();
                if (user == null) return Challenge();

                if (user.Id != userId)
                {
                    return BadRequest(new { message = "Status update failed: You are not assigned to this to-do." });
                }

                await _mediator.Send(new UpdateTodoStatusCommand(id, status));
                return Ok(new { success = true, message = "Status updated successfully." });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = $"The requested task could not be found: {ex.Message}" });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message});
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> ArchivedUnArchived(int id)
        {
            try
            {
                bool result = await _mediator.Send(new ToggleTodoArchiveCommand(id));
                if (!result) return NotFound();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ArchivedList()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Challenge();

            var todos = await _mediator.Send(new GetTodoByUserIdQuery(user.Id,true));

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
            try
            {
                await _mediator.Send(new DeleteTodoCommand(id));
            }
            catch(ArgumentNullException ex)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(ArchivedList));
        }

        [HttpPost]
        public async Task<IActionResult> QuickAssign(int userId, int projectId, int todoId)
        {
            if (!ModelState.IsValid)
            {
                TempData["AssignMemberError"] = "Assign member unsuccessful.";
                return RedirectToAction("Details", "Project", new { id = projectId });
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Challenge();

            try
            {
                await _mediator.Send(new QuickAssignTodoCommand(userId, projectId, todoId, currentUser.Id, currentUser.FullName));
            }
            catch (ArgumentNullException ex)
            {
                TempData["AssignMemberError"] = ex.Message;
            }

            return RedirectToAction("Details", "Project", new { Id = projectId });
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!ModelState.IsValid) return View();

            var todoDto = await _mediator.Send(new GetTodoByIdQuery(id));
            if (todoDto == null) return NotFound();

            
            return View(todoDto);
        }

        
        private async Task<User?> GetCurrentUserAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;

            return await _mediator.Send(new GetUserByUserIdQuery(userId));
        }

    }
}