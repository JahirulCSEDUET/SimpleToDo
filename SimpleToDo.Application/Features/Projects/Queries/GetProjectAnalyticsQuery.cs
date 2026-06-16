using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Projects.Queries
{
    public record GetProjectAnalyticsQuery(int projectId) : IRequest<ProjectAnalyticsDto>;
    public class GetProjectAnalyticsQueryHandler : IRequestHandler<GetProjectAnalyticsQuery, ProjectAnalyticsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectAnalyticsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectAnalyticsDto> Handle(GetProjectAnalyticsQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Project.GetByIdAsync(request.projectId);
            if(project == null)
            {
                throw new InvalidOperationException("Invalid project id.");
            }
            var report = new ProjectAnalyticsDto
            {
                ProjectId = request.projectId,
                ProjectName = project.Name,
                TotalTasks = project.Todos.Count,
                InProgressTasks = project.Todos.Count(t => t.Status == Status.Processing),
                UnassignedTasks = project.Todos.Count(t => t.User == null),
                CompletedTasks = project.Todos.Count(t => t.Status == Status.Completed),
                PendingTasks = project.Todos.Count(t => t.Status == Status.Pending),
                AssigneeConditions = project.ProjectMembers.Select(pm => new AssigneeMemberDto
                {
                    UserName = pm.User.FullName,
                    TodoCount = project.Todos.Count(t => t.UserId == pm.UserId && t.Status == Status.Pending),
                    InProgressCount = project.Todos.Count(t=> t.UserId== pm.UserId && t.Status==Status.Processing),
                    DoneCount = project.Todos.Count(t => t.UserId == pm.UserId && t.Status == Status.Completed)
                }).ToList()
            };
            return report;
        }
    }
}
