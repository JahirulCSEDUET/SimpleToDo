using Microsoft.EntityFrameworkCore;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly SimpleToDoDbContext _context;

        public ChatRepository(SimpleToDoDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Chat> GetByIdAsync(int id)
        {
            var chat = await _context.Chats
                .AsNoTracking()
                .Include(i=> i.Project)
                    .ThenInclude(i=> i.ProjectMembers)
                        .ThenInclude(i=>i.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            return chat;   
        }

        public async Task<IReadOnlyList<Chat>> GetByProjectIdAsync(int projectId)
        {
            var chats = await _context.Chats
                .AsNoTracking()
                .Include(i=> i.Messages)
                .Where(c => c.ProjectId == projectId)
                .ToListAsync();
            return chats;
        }

        public async Task<int> UnreadMessageCountByProjectIdAndUserId(int projectId, int userId)
        {
            return await _context.Messages
                .AsNoTracking() 
                .CountAsync(m => m.Chat.ProjectId == projectId
                      && m.UserId == userId
                      && !m.IsRead);
        }
        public async Task<Chat?> GetChatWithMembersAsync(int chatId)
        {
            return await _context.Chats
                .Include(c => c.Project)
                    .ThenInclude(p => p.ProjectMembers)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<List<Chat>> GetUserProjectChatsAsync(int userId)
        {
            return await _context.ProjectMembers
                .Where(pm => pm.UserId == userId && !pm.Project.IsDeleted)
                .Select(pm => pm.Project.Chat)
                .Where(c => c != null)
                .ToListAsync();
        }

        public async Task AddAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
        }
    }
}

