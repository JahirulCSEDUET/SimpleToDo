using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Users.Commands
{
    public record CreateUserCommand(string UserId, string FullName, string Email):IRequest<int>;
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FullName = request.FullName,
                UserId = request.UserId,
                Email = request.Email
            };
            await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return user.Id;
        }
    }
}
