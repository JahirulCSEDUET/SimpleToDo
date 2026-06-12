using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    public class QueryController : Controller
    {
        private readonly IQueryService _queryService;
        private readonly IUserService _userService;
        public QueryController(IQueryService queryService, IUserService userService)
        {
            _queryService = queryService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int todoId, string body)
        {
            if (!ModelState.IsValid)
            {
                TempData["QueryError"] = "Query message body content cannot be empty.";
                return RedirectToAction("Details", "Todo", new {Id= todoId});
            }
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
            var query = new Query
            {
                Body = body,
                TodoId = todoId,
                UserId = user.Id
            };
            await _queryService.AddAsync(query);
            return RedirectToAction("Details", "Todo", new { Id = todoId });
        }
    }
}
