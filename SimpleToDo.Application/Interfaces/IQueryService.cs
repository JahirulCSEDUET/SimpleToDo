using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IQueryService
    {
        Task<Query> AddAsync(Query query);
        Task<IReadOnlyList<Query>> GetByUserIdAsync(int userId);
    }
}
