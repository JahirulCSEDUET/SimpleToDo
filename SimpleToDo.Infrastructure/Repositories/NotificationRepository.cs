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

        public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications.Where(i => i.UserId == userId).ToListAsync();
        }
    }
}
