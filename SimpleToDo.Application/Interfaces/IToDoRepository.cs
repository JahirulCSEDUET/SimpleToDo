using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IToDoRepository
    {
        Task<ToDoItem> AddAsync(ToDoItem item);
        Task<bool> DeleteAsync(ToDoItem item);
        Task UpdateAsync(ToDoItem item);
        Task<ToDoItem> GetByIdAsync(int id);
        Task<ToDoItem> GetByUserIdAsync(string userId);
        Task<IReadOnlyList<ToDoItem>> GetAllASync();
    }
}
