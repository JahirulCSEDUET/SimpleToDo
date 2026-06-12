using Microsoft.EntityFrameworkCore;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class ToDoRepository :Repository<Todo>, IToDoRepository
    {
        private readonly SimpleToDoDbContext _context;
        public ToDoRepository(SimpleToDoDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Todo>> GetByUserIdWithProjectAsync(int userId, bool isArchived)
        {
            return await _context.Todos
                .Where(t=> t.UserId == userId && t.IsArchived== isArchived)
                .Include(t=>t.Project)
                .AsNoTracking()
                .ToListAsync();

        }
    }
}
