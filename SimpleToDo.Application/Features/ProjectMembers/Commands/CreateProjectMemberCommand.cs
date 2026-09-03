using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.ProjectMembers.Commands
{
    public record CreateProjectMemberCommand(int projectId, string email, int loginUserId, string loginUserName, string role) : IRequest<int>;
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
            var loginUser = await _unitOfWork.ProjectMember.GetProjectMemberByIdAsync(request.projectId, request.loginUserId);
            if (loginUser.Role != Role.Admin)
            {
                throw new InvalidOperationException($"Only Admin can add member.");
            }
            if (user == null)
            {
                throw new InvalidOperationException($"User with email: {request.email} is not found in the system.");
            }
            var project = await _unitOfWork.Project.GetByIdAsync(request.projectId);
            if (project == null)
            {
                throw new InvalidOperationException("Project is not found.");
            }
            var existedMember = await _unitOfWork.ProjectMember.GetProjectMemberByIdAsync(request.projectId, user.Id);
            if(existedMember != null)
            {
                throw new InvalidOperationException($"Member with email {request.email} already exist.");
            }
            var projectMember = new ProjectMember
            {
                UserId = user.Id,
                ProjectId = request.projectId
            };
            if(request.role == Role.Admin.ToString())
            {
                projectMember.Role = Role.Admin;
            }
            else if(request.role == Role.Contributor.ToString())
            {
                projectMember.Role = Role.Contributor;
            }
            else if(request.role == Role.Coordinator.ToString())
            {
                projectMember.Role = Role.Coordinator;
            }
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
