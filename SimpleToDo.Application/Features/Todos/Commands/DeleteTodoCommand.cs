using MediatR;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Todos.Commands
{
    public record DeleteTodoCommand(int id) : IRequest<bool>;
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTodoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.id);
            if(todo == null)
            {
                throw new ArgumentNullException("Todo id is not valid.");
            }
            _unitOfWork.Todo.Delete(todo);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}
