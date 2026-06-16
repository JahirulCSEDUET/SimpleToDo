using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Notifications.Queries
{
    public record GetNotificationByUserIdQuery(int UserId) : IRequest<IReadOnlyList<NotificationDto>>;
    public class GetNotificationByUserIdQueryHandler : IRequestHandler<GetNotificationByUserIdQuery, IReadOnlyList<NotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNotificationByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<NotificationDto>> Handle(GetNotificationByUserIdQuery request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.Notification.GetByUserIdAsync(request.UserId);
            return _mapper.Map<IReadOnlyList<NotificationDto>>(notification);
        }
    }
}
