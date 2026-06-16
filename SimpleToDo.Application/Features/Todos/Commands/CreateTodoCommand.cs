using AutoMapper;
using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Todos.Commands
{
    public record CreateTodoCommand(string Title, string Description, string? FileName, string? FilePath, int ProjectId,
        int CreatorId, string CreatorName):IRequest<int>;
    
    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTodoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = _mapper.Map<Todo>(request);
            await _unitOfWork.Todo.AddAsync(todo);
            await _unitOfWork.SaveAsync();
            return todo.Id;
        }
    }

}
