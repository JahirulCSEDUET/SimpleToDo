using MediatR;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Notifications.Commands
{
    public record MarkAllNotificationReadCommand(int userId) : IRequest;
    public class MarkAllNotificationReadCommandHandler : IRequestHandler<MarkAllNotificationReadCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkAllNotificationReadCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarkAllNotificationReadCommand request, CancellationToken cancellationToken)
        {
            var notifications = await _unitOfWork.Notification.GetByUserIdAsync(request.userId);
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _unitOfWork.SaveAsync();
        }
    }
}
