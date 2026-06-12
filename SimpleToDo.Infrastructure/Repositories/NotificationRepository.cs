using Microsoft.EntityFrameworkCore;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly SimpleToDoDbContext _context;

        public NotificationRepository(SimpleToDoDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<int> CountUnreadNotificationByUserIdAsync(int userId)
        {
            return await _context.Notifications.Where(i=> !i.IsRead && i.UserId==userId).CountAsync();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(i=> i.Id==id);
        }

        public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications.Where(i => i.UserId == userId).ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadNotificationByIdAsync(int userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        }
    }
}
