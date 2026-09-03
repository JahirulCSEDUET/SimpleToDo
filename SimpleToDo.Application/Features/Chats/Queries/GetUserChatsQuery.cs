using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Chats.Queries
{
    public class GetUserChatsQuery : IRequest<UserChatsVm>
    {
        public int UserId { get; set; }
    }

    public class UserChatsVm
    {
        public int TotalUnread { get; set; }
        public List<UserChatSummaryDto> Chats { get; set; } = new();
    }

    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, UserChatsVm>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserChatsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<UserChatsVm> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _unitOfWork.Chats.GetUserProjectChatsAsync(request.UserId);
            var result = new UserChatsVm();

            if (chats == null || !chats.Any())
            {
                return result;
            }

            foreach (var chat in chats.Where(c => c != null))
            {
                var unread = await _unitOfWork.Messages.GetUnreadCountAsync(chat.Id, request.UserId);
                var lastMsg = await _unitOfWork.Messages.GetLastMessageAsync(chat.Id, request.UserId);

                result.TotalUnread += unread;
                result.Chats.Add(new UserChatSummaryDto
                {
                    ChatId = chat.Id,
                    ProjectId = chat.ProjectId,
                    ChatName = chat.Name ?? "Project Discussion",
                    UnreadCount = unread,
                    LastMessage = lastMsg?.Body,
                    LastMessageTime = lastMsg?.CreatedDateTime.ToString("MMM dd, hh:mm tt")
                });
            }

            return result;
        }
    }
}
