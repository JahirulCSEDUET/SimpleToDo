using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IProjectService
    {
        Task<Project> AddAsync(Project project);
        Task<bool> DeleteAsync(int id);
        Task UpdateAsync(Project project);
        Task<IReadOnlyList<Project>> GetAllAsync();
        Task<IReadOnlyList<Project>> GetByMemberIdAsync(int userId);
        Task<Project> GetByIdAsync(int id);        
        Task<Project> GetByIdWithMembersAsync(int projectId);
        Task<Project> GetByIdWithTodoAsync(int projectId);
    }
}
