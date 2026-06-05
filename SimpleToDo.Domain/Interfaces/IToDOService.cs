using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IToDoService
    {
        Task<ToDoItem> AddAsync(ToDoItem item);
        Task<bool> DeleteAsync(int id);
        Task UpdateStatus(int id, string status);
        Task<ToDoItem> GetByIdAsync(int id);
        Task<IReadOnlyList<ToDoItem>> GetByUserIdAsync(string userId);
        Task<IReadOnlyList<ToDoItem>> GetAllASync();
    }
}
