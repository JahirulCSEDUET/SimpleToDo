using MediatR;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Projects.Commands
{    
    public record SafeDeleteProjectCommand(int id, int userId) : IRequest<bool>;
    public class SafeDeleteProjectCommandHandler : IRequestHandler<SafeDeleteProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SafeDeleteProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SafeDeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project =await _unitOfWork.Project.GetByIdAsync(request.id);
            if (project == null)
            {
                throw new ArgumentNullException($"Project with id {request.id} is not exist.");
            }
            var role = project.ProjectMembers.FirstOrDefault(m=> m.UserId== request.userId)?.Role;
            if(role != Role.Admin)
            {
                throw new InvalidOperationException("You are not admin on this workspace");
            }
            if (!project.IsDeleted && project.Todos.Where(t => t.Status != Status.Completed).Any())
            {
                throw new InvalidOperationException("All Task in this workspace are not completed.");
            }
            project.IsDeleted = project.IsDeleted?false:true;
            _unitOfWork.Project.Update(project);
            return await _unitOfWork.SaveAsync()>0;
        }
    }
}
