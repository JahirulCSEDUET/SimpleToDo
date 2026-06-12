using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IToDoRepository Todo { get; }
        IUserRepository User { get; }
        IProjectRepository Project { get; }
        IProjectMemberRepository ProjectMember { get; }
        IQueryRepository Query { get; }
        Task<int> SaveAsync();
    }
}
