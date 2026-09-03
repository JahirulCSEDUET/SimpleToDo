using MediatR;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Application.DTOs;
namespace SimpleToDo.Application.Features.Messages.Commands
{
    public class SendChatMessageCommand : IRequest<SendChatMessageResult>
    {
        public int ChatId { get; set; }
        public string Body { get; set; } = null!;
        public int CurrentUserId { get; set; }
        public string CurrentUserFullName { get; set; } = null!;
    }


    public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, SendChatMessageResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendChatMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SendChatMessageResult> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Body))
            {
                return new SendChatMessageResult { Success = false, ErrorMessage = "Message body cannot be empty." };
            }

            var chat = await _unitOfWork.Chats.GetChatWithMembersAsync(request.ChatId);

            if (chat == null || !chat.Project.ProjectMembers.Any(pm => pm.UserId == request.CurrentUserId))
            {
                return new SendChatMessageResult { Success = false, ErrorMessage = "Chat not found or access denied." };
            }

            var trimmedBody = request.Body.Trim();
            var now = DateTime.Now;

            var messages = chat.Project.ProjectMembers.Select(member => new Message
            {
                ChatId = chat.Id,
                UserId = member.UserId,
                SenderId = request.CurrentUserId,
                SenderName = request.CurrentUserFullName,
                Body = trimmedBody,
                IsRead = member.UserId == request.CurrentUserId,
                CreatedDateTime = now
            }).ToList();

            chat.LastUpdateDateTime = now;

            await _unitOfWork.Messages.AddRangeAsync(messages);
            await _unitOfWork.SaveAsync();

            return new SendChatMessageResult
            {
                Success = true,
                Body = trimmedBody,
                FormattedTime = now.ToString("hh:mm tt")
            };
        }
    }
}
