using MediatR;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Todos.Commands
{
    public record UpdateTodoCommand(int Id, Status Status) : IRequest;
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTodoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.Id);
            if(todo == null)
            {
                throw new KeyNotFoundException($"Todo with ID {request.Id} was not found.");
            }
            todo.Status = request.Status;
            _unitOfWork.Todo.Update(todo);
            await _unitOfWork.SaveAsync();
        }
    }
}
