using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Features.Users.Queries;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IActionResult> IndexAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;

            var user = await _mediator.Send(new GetUserInformationByIdQuery(userId));
            return View(user);
        }
    }
}
