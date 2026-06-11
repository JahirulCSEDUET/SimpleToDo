using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Domain.Interfaces
{
    public interface INotificationRepository:IRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId);
    }
}
