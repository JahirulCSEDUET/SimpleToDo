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

        public async Task<int> CountUnreadNotificationByUserIdAsync(int userId)
        {
            return await _unitOfWork.Notification.CountUnreadNotificationByUserIdAsync(userId);
        }

        public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId)
        {
            return await _unitOfWork.Notification.GetByUserIdAsync(userId);
        }

        public async Task MarkAsReadByIdAsync(int id)
        {
            var notification = await _unitOfWork.Notification.GetByIdAsync(id);
            notification.IsRead = true;
            _unitOfWork.Notification.Update(notification);
            await _unitOfWork.SaveAsync();
        }

        public async Task MarkAsReadByUserIdAsync(int userId)
        {
            var items = await _unitOfWork.Notification.GetUnreadNotificationByIdAsync(userId);
            foreach (var item in items)
            {
                item.IsRead = true;
            }
            //_unitOfWork.Notification.UpdateRange(items);
            await _unitOfWork.SaveAsync();
        }
    }
}
