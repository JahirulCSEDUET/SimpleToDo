using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.ProjectMembers.Queries
{
    public record GetProjectMemberByUserAndProjectIdQuery(int userId, int projectId) : IRequest<ProjectMemberDto>;
    public class GetProjectMemberByUserAndProjectIdQueryHandler : IRequestHandler<GetProjectMemberByUserAndProjectIdQuery, ProjectMemberDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProjectMemberByUserAndProjectIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectMemberDto> Handle(GetProjectMemberByUserAndProjectIdQuery request, CancellationToken cancellationToken)
        {
            var projectMember = await _unitOfWork.ProjectMember.GetProjectMemberByIdAsync(request.projectId, request.userId);
            return _mapper.Map<ProjectMemberDto>(projectMember);
        }
    }
}
