using MediatR;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Notifications.Queries
{
    public record GetUnreadNotificationCountQuery(int userId) : IRequest<int>;
    public class GetUnreadNotificationCountQueryhandler : IRequestHandler<GetUnreadNotificationCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUnreadNotificationCountQueryhandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Notification.CountUnreadNotificationByUserIdAsync(request.userId);
        }
    }
}
