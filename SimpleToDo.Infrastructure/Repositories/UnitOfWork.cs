using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;

namespace SimpleToDo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleToDoDbContext _context;
        public UnitOfWork(SimpleToDoDbContext context, IToDoRepository todo, IProjectMemberRepository projectMember, IProjectRepository project, IUserRepository user, IQueryRepository query, INotificationRepository notification)
        {
            _context = context;
            Todo = todo;
            ProjectMember = projectMember;
            Project = project;
            User = user;
            Query = query;
            Notification = notification;
        }

        public IToDoRepository Todo { get;}

        public IUserRepository User { get; }

        public IProjectRepository Project { get; }

        public IProjectMemberRepository ProjectMember { get; }
        public IQueryRepository Query { get; }
        public INotificationRepository Notification { get; }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
