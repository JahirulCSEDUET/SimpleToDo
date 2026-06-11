using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IToDoRepository : IRepository<Todo>
    {
        Task<IReadOnlyList<Todo>> GetByUserIdWithProjectAsync(int userId, bool isArchived);
        Task<Todo> GetByIdAsync(int id);
    }
}
