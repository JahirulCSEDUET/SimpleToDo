using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IProjectMemberService
    {
        Task<ProjectMember> AddAsync(ProjectMember projectMember);
    }
}
