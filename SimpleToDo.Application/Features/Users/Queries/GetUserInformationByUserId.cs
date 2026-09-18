using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs.SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Users.Queries
{
    public record GetUserInformationByIdQuery(string userId) : IRequest<UserProfileDto>;
    public class GetUserInformationByIdQueryHandler : IRequestHandler<GetUserInformationByIdQuery, UserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserInformationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetUserInformationByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.GetByUserIdAsync(request.userId);
            return new UserProfileDto
            {
                UserId = user.UserId,
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TotalWorkspaces = user.ProjectMembers.Count(),
                TotalAssignedTasks = user.Todos.Count(),
                CompletedTasksCount = user.Todos.Count(t=> t.Status==Status.Completed),
                PendingTasksCount = user.Todos.Count(t=> t.Status == Status.Pending || t.Status == Status.Processing)
            };
        }
    }
}
