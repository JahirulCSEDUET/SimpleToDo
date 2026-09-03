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
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly SimpleToDoDbContext _context;
        public ProjectRepository(SimpleToDoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await _context.Projects
                .AsNoTracking()
                .Include(i=> i.ProjectMembers.OrderBy(pm=> pm.Role))
                    .ThenInclude(pm=>pm.User)
                .Include(p=> p.Todos)
                    .ThenInclude(t=>t.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Project> GetByIdForUpdateAsync(int id)
        {
            return await _context.Projects
                .Include(i=> i.ProjectMembers.OrderBy(pm=> pm.Role))
                    .ThenInclude(pm=>pm.User)
                .Include(p=> p.Todos)
                    .ThenInclude(t=>t.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Project>> GetByUserIdAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.ProjectMembers.Any(pm => pm.UserId == userId))
                .Include(p => p.ProjectMembers)
                    .ThenInclude(pm => pm.User)
                .ToListAsync();
        }
    }
}
