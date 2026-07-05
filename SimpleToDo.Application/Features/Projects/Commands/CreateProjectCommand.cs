using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SimpleToDo.Application.Features.Projects.Commands
{
    public record CreateProjectCommand(string Name, int userId) : IRequest<int>;
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                IsDeleted =false,
                Status = ProjectStatus.Running,
                ProjectMembers = new List<ProjectMember>
                {
                    new ProjectMember { UserId = request.userId, Role = Role.Admin}
                }
            };
            await _unitOfWork.Project.AddAsync(project);
            await _unitOfWork.SaveAsync();
            return project.Id;
        }
    }

}
