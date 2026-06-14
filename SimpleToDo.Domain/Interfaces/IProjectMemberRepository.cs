using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IProjectMemberRepository:IRepository<ProjectMember>
    {
        Task<ProjectMember> GetProjectMemberByIdAsync(int projectId,  int userId);
    }
}
