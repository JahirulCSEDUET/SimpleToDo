using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Application.Features.Chats.Queries;
using SimpleToDo.Application.Features.Messages.Commands;
using SimpleToDo.Application.Features.Users.Queries;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Web.Hubs;
using System.Security.Claims;

namespace SimpleToDo.Web.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatHub> _hubContext;

        // UserManager removed from constructor
        public MessageController(IMediator mediator, IHubContext<ChatHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;

            return await _mediator.Send(new GetUserByUserIdQuery(userId));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChats()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();

            var result = await _mediator.Send(new GetUserChatsQuery { UserId = user.Id });
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessages(int chatId)
        {
            var user = await GetCurrentUserAsync();
            if (user==null) return Unauthorized();

            var messages = await _mediator.Send(new GetChatMessagesQuery { ChatId = chatId, UserId = user.Id });
            return Json(messages);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatFeed(int chatId)
        {
            try
            {
                // 1. Get current user
                var user = await GetCurrentUserAsync();
                if (user == null) return Unauthorized();

                // 2. Fetch messages via MediatR
                var messages = await _mediator.Send(new GetChatMessagesQuery
                {
                    ChatId = chatId,
                    UserId = user.Id
                });

                return PartialView("~/Views/Shared/_ChatMessageFeed.cshtml", messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendChatMessage([FromBody] SendChatMessageRequest request)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();

            var command = new SendChatMessageCommand
            {
                ChatId = request.ChatId,
                Body = request.Body,
                CurrentUserId = user.Id,
                CurrentUserFullName = User.Identity?.Name ?? "Member",
               

            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            var chat = await _mediator.Send(new GetChatByIdQuery
            {
                Id = request.ChatId
            });
            var recipientUserIds = chat.UserIds;
            
            if (recipientUserIds.Any())
            {
                await _hubContext.Clients.Users(recipientUserIds)
                    .SendAsync("ReceiveMessageNotification", request.ChatId);
                await _hubContext.Clients.Users(recipientUserIds)
                    .SendAsync("UpdateMessageBadge");
            }
            return Ok(new
            {
                senderName = User.Identity?.Name ?? "Member",
                body = result.Body,
                formattedTime = result.FormattedTime
            });
        }
    }
}