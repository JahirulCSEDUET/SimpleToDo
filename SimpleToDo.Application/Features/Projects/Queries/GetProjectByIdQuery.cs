using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text; 

namespace SimpleToDo.Application.Features.Projects.Queries
{
    public record GetProjectByIdQuery(int id) : IRequest<ProjectDto>;
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        { 
            var project = await _unitOfWork.Project.GetByIdAsync(request.id);
            return _mapper.Map<ProjectDto>(project);
        }
    }
}
