using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            await _unitOfWork.Notification.AddAsync(notification);
            await _unitOfWork.SaveAsync();
            return notification;
        }

        public async Task<IReadOnlyList<Notification>> GetByUserId(int userId)
        {
            return await _unitOfWork.Notification.GetByUserIdAsync(userId);
        }
    }
}
