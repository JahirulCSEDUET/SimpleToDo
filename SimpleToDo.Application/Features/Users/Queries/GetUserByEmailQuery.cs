using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Users.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<User>;
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.User.GetUserByEmailAsync(request.Email);
        }
    }
}
