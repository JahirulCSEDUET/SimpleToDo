using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IToDoService
    {
        Task<Todo> AddAsync(Todo item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ArchiveUnarchivedAsync(int id);
        Task UpdateStatus(int id, string status);
        Task<Todo> GetByIdAsync(int id);
        Task<IReadOnlyList<Todo>> GetByUserIdAsync(string userId, bool isArchived);
        Task<IReadOnlyList<Todo>> GetAllASync();
    }
}
