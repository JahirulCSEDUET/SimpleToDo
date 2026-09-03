using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Chats.Queries
{
    public class GetChatMessagesQuery : IRequest<List<ChatMessageDetailDto>>
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
    }

    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, List<ChatMessageDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChatMessagesQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<ChatMessageDetailDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Messages.MarkChatMessagesAsReadAsync(request.ChatId, request.UserId);
            var messages = await _unitOfWork.Messages.GetMessagesForUserAsync(request.ChatId, request.UserId);

            return messages.Select(m => new ChatMessageDetailDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderName = m.SenderName,
                Body = m.Body,
                IsMe = m.SenderId == request.UserId,
                CreatedDateTime = m.CreatedDateTime
            }).ToList();
        }
    }
}
