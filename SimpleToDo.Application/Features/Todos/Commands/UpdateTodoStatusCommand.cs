using MediatR;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Todos.Commands
{
    public record UpdateTodoStatusCommand(int Id, string Status) : IRequest;
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoStatusCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTodoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateTodoStatusCommand request, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.Id);
            if(todo == null)
            {
                throw new KeyNotFoundException($"Todo with ID {request.Id} was not found.");
            }
            if(request.Status == "Pending")
            {
                todo.Status = Status.Pending;
            }
            else if(request.Status == "Processing")
            {
                todo.Status = Status.Processing;
            }
            else if(request.Status == "Completed")
            {
                todo.Status = Status.Completed;
            }
            else
            {
                throw new InvalidOperationException($"Status: {request.Status} is not valid.");
            }
            _unitOfWork.Todo.Update(todo);
            await _unitOfWork.SaveAsync();
        }
    }
}
