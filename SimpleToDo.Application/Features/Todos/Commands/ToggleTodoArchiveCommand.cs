using MediatR;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Todos.Commands
{
    public record ToggleTodoArchiveCommand(int id) : IRequest<bool>;
    public class ToggleTodoArchiveCommandHandler : IRequestHandler<ToggleTodoArchiveCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleTodoArchiveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ToggleTodoArchiveCommand request, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.id);
            if (todo == null)
            {
                throw new KeyNotFoundException($"Todo with ID {request.id} was not found.");
            }
            if (todo.IsArchived)
            {
                todo.IsArchived = false;
            }
            else todo.IsArchived = true;
            _unitOfWork.Todo.Update(todo);
            return await _unitOfWork.SaveAsync()>0;
        }

    }
}
