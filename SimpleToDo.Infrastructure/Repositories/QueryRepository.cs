using Microsoft.EntityFrameworkCore;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class QueryRepository : Repository<Query>, IQueryRepository
    {
        private readonly SimpleToDoDbContext _context;
        public QueryRepository(SimpleToDoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Query>> GetByUserIdAsync(int userId)
        {
            return await _context.Queries
                .Where(q=> q.UserId==userId)
                .Include(q => q.User)
                .Include(q => q.Todo)
                .ToListAsync();
        }
    }
}
