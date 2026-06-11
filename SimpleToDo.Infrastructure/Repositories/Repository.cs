using Microsoft.EntityFrameworkCore;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        public Repository(SimpleToDoDbContext context)
        {
            _dbSet = context.Set<T>();
            
        }
        public async Task<T> AddAsync(T item)
        {
            await _dbSet.AddAsync(item);
            return item;
        }

        public void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        public IQueryable<T> Query()
        {
            return  _dbSet.AsQueryable();
        }

        public void Update(T item)
        {
            _dbSet.Update(item);
        }

    }
}
