using Microsoft.EntityFrameworkCore;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly SimpleToDoDbContext _context;

        public ToDoRepository(SimpleToDoDbContext context)
        {
            _context = context;
        }

        public async Task<Todo> AddAsync(Todo item)
        {
            await _context.Todos.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(Domain.Entities.Todo item)
        {
            _context.Todos.Remove(item);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<IReadOnlyList<Todo>> GetAllASync()
        {
            return await _context.Todos.AsNoTracking().ToListAsync();
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _context.Todos.AsNoTracking().FirstOrDefaultAsync(i=> i.Id == id);
        }

        public async Task<IReadOnlyList<Todo>> GetByUserIdAsync(string userId, bool isArchived)
        {
            return await _context.Todos.AsNoTracking().Where(i => i.UserId == userId && i.IsArchived== isArchived).ToListAsync();
        }

        public async  Task UpdateAsync(Domain.Entities.Todo item)
        {
            _context.Todos.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
