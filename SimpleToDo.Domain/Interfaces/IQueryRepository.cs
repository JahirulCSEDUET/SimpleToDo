using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IQueryRepository:IRepository<Query>
    {
        Task<IReadOnlyList<Query>> GetByUserIdAsync(int userId); 
    } 
}
