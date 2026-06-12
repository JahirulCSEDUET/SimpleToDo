using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Application.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> AddAsync(Notification notification);
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId);
        Task<int> CountUnreadNotificationByUserIdAsync(int userId);
        Task MarkAsReadByUserIdAsync(int userId);
        Task MarkAsReadByIdAsync(int id);
    }
}
