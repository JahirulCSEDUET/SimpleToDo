using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Users.Queries
{
    public record GetUserByUserQuery(string UserId) : IRequest<User>;
    public class GetUserByUserQueryHandler : IRequestHandler<GetUserByUserQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserByUserQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.User.GetByUserIdAsync(request.UserId);
        }
    }
}
