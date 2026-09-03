using Microsoft.EntityFrameworkCore;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        private readonly SimpleToDoDbContext _context;
        public ProjectMemberRepository(SimpleToDoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProjectMember> GetProjectMemberByIdAsync(int projectId, int userId)
        {
            return await _context.ProjectMembers
                .AsNoTracking()
                .Include(pm=> pm.Project)
                .Include(pm=> pm.User)
                .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
            
        }
    }
}
