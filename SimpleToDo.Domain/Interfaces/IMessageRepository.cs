using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IMessageRepository:IRepository<Message>
    {
        Task AddRangeAsync(IEnumerable<Message> messages);
        Task<List<Message>> GetMessagesForUserAsync(int chatId, int userId);
        Task<int> GetUnreadCountAsync(int chatId, int userId);
        Task<Message?> GetLastMessageAsync(int chatId, int userId);
        Task MarkChatMessagesAsReadAsync(int chatId, int userId);
    }
}
