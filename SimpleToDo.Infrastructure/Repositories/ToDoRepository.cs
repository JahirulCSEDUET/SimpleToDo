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

        public async Task<ToDoItem> AddAsync(ToDoItem item)
        {
            await _context.ToDoItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(ToDoItem item)
        {
            _context.ToDoItems.Remove(item);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<IReadOnlyList<ToDoItem>> GetAllASync()
        {
            return await _context.ToDoItems.AsNoTracking().ToListAsync();
        }

        public async Task<ToDoItem> GetByIdAsync(int id)
        {
            return await _context.ToDoItems.AsNoTracking().FirstOrDefaultAsync(i=> i.Id == id);
        }

        public async Task<IReadOnlyList<ToDoItem>> GetByUserIdAsync(string userId, bool isArchived)
        {
            return await _context.ToDoItems.AsNoTracking().Where(i => i.UserId == userId && i.IsArchived== isArchived).ToListAsync();
        }

        public async Task UpdateAsync(ToDoItem item)
        {
            _context.ToDoItems.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
