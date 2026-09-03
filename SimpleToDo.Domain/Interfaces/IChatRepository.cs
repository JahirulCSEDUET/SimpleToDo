using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IChatRepository:IRepository<Chat>
    {
        Task<Chat> GetByIdAsync(int id);
        Task<IReadOnlyList<Chat>> GetByProjectIdAsync(int projectId);
        Task<int> UnreadMessageCountByProjectIdAndUserId(int projectId, int userId);
        Task<Chat?> GetChatWithMembersAsync(int chatId);
        Task<List<Chat>> GetUserProjectChatsAsync(int userId);

    }
}
