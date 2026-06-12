using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Domain.Interfaces
{
    public interface INotificationRepository:IRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId);
        Task<int> CountUnreadNotificationByUserIdAsync(int userId);
        Task<List<Notification>> GetUnreadNotificationByIdAsync(int userId);
        Task<Notification> GetByIdAsync(int id);
    }
}
