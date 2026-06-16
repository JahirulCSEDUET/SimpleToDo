using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Todos.Queries
{
    public record GetTodoByUserIdQuery(int userId, bool isArchive) : IRequest<IReadOnlyList<TodoDto>>;
    public class GetTodoByUserIdQueryHandler : IRequestHandler<GetTodoByUserIdQuery, IReadOnlyList<TodoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetTodoByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TodoDto>> Handle(GetTodoByUserIdQuery request, CancellationToken cancellationToken)
        {
            var todos = await _unitOfWork.Todo.GetTodoByUserId(request.userId, request.isArchive);
            return _mapper.Map<IReadOnlyList<TodoDto>>(todos);
        }
    }
}
