using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IToDoRepository
    {
        Task<ToDoItem> AddAsync(ToDoItem item);
        Task<bool> DeleteAsync(ToDoItem item);
        Task UpdateAsync(ToDoItem item);
        Task<ToDoItem> GetByIdAsync(int id);
        Task<IReadOnlyList<ToDoItem>> GetByUserIdAsync(string userId, bool isArchived);
        Task<IReadOnlyList<ToDoItem>> GetAllASync();
    }
}
