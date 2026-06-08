using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IToDoRepository
    {
        Task<Todo> AddAsync(Todo item);
        Task<bool> DeleteAsync(Todo item);
        System.Threading.Tasks.Task UpdateAsync(Todo item);
        Task<Todo> GetByIdAsync(int id);
        Task<IReadOnlyList<Todo>> GetByUserIdAsync(string userId, bool isArchived);
        Task<IReadOnlyList<Todo>> GetAllASync();
    }
}
