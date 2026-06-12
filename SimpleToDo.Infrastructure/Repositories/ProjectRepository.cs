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
        public async Task<Project> GetByIdWithMemberAndTodoAsync(int projectId)
        {
            return await _context.Projects
                .Include(p => p.ProjectMembers)
                    .ThenInclude(pm => pm.User)
                .Include(p=> p.Todos)
                    .ThenInclude(pm => pm.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == projectId);
                
        }
        public async Task<IReadOnlyList<Project>> GetByMemberIdAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.ProjectMembers.Any(pm => pm.UserId == userId))
                .Include(p => p.ProjectMembers)
                    .ThenInclude(pm => pm.User)
                .ToListAsync();
        }
    }
}
