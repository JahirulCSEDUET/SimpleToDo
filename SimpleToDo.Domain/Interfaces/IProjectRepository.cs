using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IProjectRepository:IRepository<Project>
    {
        Task<IReadOnlyList<Project>> GetByUserIdAsync(int userId);
        Task<Project> GetByIdAsync(int id);
    }
}
