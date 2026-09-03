using Microsoft.EntityFrameworkCore;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class MessageRepository:Repository<Message>, IMessageRepository
    {
        private readonly SimpleToDoDbContext _context;

        public MessageRepository(SimpleToDoDbContext context):base(context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(IEnumerable<Message> messages)
        {
            await _context.Messages.AddRangeAsync(messages);
        }

        public async Task<List<Message>> GetMessagesForUserAsync(int chatId, int userId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId && m.UserId == userId)
                .OrderBy(m => m.CreatedDateTime)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int chatId, int userId   )
        {
            return await _context.Messages
                .CountAsync(m => m.ChatId == chatId && m.UserId == userId && !m.IsRead);
        }

        public async Task<Message?> GetLastMessageAsync(int chatId, int userId      )
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId && m.UserId == userId)
                .OrderByDescending(m => m.CreatedDateTime)
                .FirstOrDefaultAsync(   );
        }

        public async Task MarkChatMessagesAsReadAsync(int chatId, int userId)
        {
            await _context.Messages
                .Where(m => m.ChatId == chatId && m.UserId == userId && !m.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.IsRead, true));
        }
    }
}
