using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Application.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> AddAsync(Notification notification);
        Task<IReadOnlyList<Notification>> GetByUserId(int userId);
    }
}
