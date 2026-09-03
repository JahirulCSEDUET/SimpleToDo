using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Projects.Commands
{
    
    public record UpdateProjectCommand(int id, string name, string status) : IRequest;
    public class UpdateProjectStatusCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IUnitOfWork _unitOfWork; 

        public UpdateProjectStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project =await _unitOfWork.Project.GetByIdForUpdateAsync(request.id);
            if (project == null)
            {
                throw new ArgumentNullException($"Project with id {request.id} is not exist.");
            }
            if (request.status == ProjectStatus.Completed.ToString() && project.Todos.Where(t => t.Status != Status.Completed).Any())
            {
                throw new InvalidOperationException("All Task in this workspace are not completed.");
            }
            if (request.status == ProjectStatus.Running.ToString())
            {
                project.Status = ProjectStatus.Running;
            }
            else if(request.status == ProjectStatus.Completed.ToString())
            {
                project.Status = ProjectStatus.Completed;
            }
            else if(request.status == ProjectStatus.Postponed.ToString())
            {
                project.Status = ProjectStatus.Postponed;
            }
            project.Name = request.name;
            //_unitOfWork.Project.Update(project);
            await _unitOfWork.SaveAsync();
        }
    }
}
