using MediatR;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Notifications.Commands
{
    public record MarkNotifiicationAsReadByIdCommand(int id) : IRequest;
    public class MarkNotifiicationAsReadByIdCommandHandler : IRequestHandler<MarkNotifiicationAsReadByIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotifiicationAsReadByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarkNotifiicationAsReadByIdCommand request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.Notification.GetByIdAsync(request.id);
            notification.IsRead = true;
            _unitOfWork.Notification.Update(notification);
            await _unitOfWork.SaveAsync();
        }
    }
}
