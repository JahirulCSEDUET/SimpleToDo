using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Todos.Queries
{
    public record GetTodoByIdQuery(int Id) : IRequest<TodoDto>;
    public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetTodoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TodoDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.Id);
            return _mapper.Map<TodoDto>(todo);
        }
    }
}
