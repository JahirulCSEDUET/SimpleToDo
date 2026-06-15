using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.ProjectMembers.Commands
{
    public record CreateProjectMemberCommand(int projectId, string email, int loginUserId, string loginUserName) : IRequest<int>;
    public class CreateProjectMemberCommandHandler : IRequestHandler<CreateProjectMemberCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectMemberCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.GetUserByEmailAsync(request.email);
            if (user == null)
            {
                throw new InvalidOperationException("User with this email is not found in the system.");
            }
            var project = await _unitOfWork.Project.GetByIdAsync(request.projectId);
            if (project == null)
            {
                throw new InvalidOperationException("Project is not found.");
            }
            var existedMember = await _unitOfWork.ProjectMember.GetProjectMemberByIdAsync(request.projectId, user.Id);
            if(existedMember != null)
            {
                throw new InvalidOperationException("Member already exist.");
            }
            var projectMember = new ProjectMember
            {
                UserId = user.Id,
                ProjectId = request.projectId,
                Role = Role.Contributor
            };
            await _unitOfWork.ProjectMember.AddAsync(projectMember);


            var notification = new Notification
            {
                Title = "Added to Workspace",
                Message = $"{request.loginUserName} added you in workspace {project.Name}.",
                RedirectLink = RedirectLink.Project,
                UserId = user.Id,
                IsRead = false,
                CreatedTime = DateTime.Now,
                RedirectId = request.projectId
            };
            await _unitOfWork.Notification.AddAsync(notification);
            await _unitOfWork.SaveAsync();
            return projectMember.Id;
        }
    }
}
