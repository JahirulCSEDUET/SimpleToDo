using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.ProjectMembers.Commands
{
    public record CreateProjectMemberCommand(int userId, int projectId) : IRequest<int>;
    public class CreateProjectMemberCommandHandler : IRequestHandler<CreateProjectMemberCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectMemberCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var projectMember = new ProjectMember
            {
                UserId = request.userId,
                ProjectId = request.projectId,
                Role = Domain.Enums.Role.Contributor
            };
            await _unitOfWork.ProjectMember.AddAsync(projectMember);
            await _unitOfWork.SaveAsync();
            return projectMember.Id;
        }
    }
}
