using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IToDoService
    {
        Task<ToDoItem> AddAsync(ToDoItem item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ArchiveUnarchivedAsync(int id);
        Task UpdateStatus(int id, string status);
        Task<ToDoItem> GetByIdAsync(int id);
        Task<IReadOnlyList<ToDoItem>> GetByUserIdAsync(string userId, bool isArchived);
        Task<IReadOnlyList<ToDoItem>> GetAllASync();
    }
}
