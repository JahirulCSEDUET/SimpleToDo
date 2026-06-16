using AutoMapper;
using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Queries.Commands
{
    public record CreateQueryCommand(string Body, int TodoId, int UserId, string? FilePath = null, string? FileName = null) : IRequest<int>;
    public class CreateQueryCommandHandler : IRequestHandler<CreateQueryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateQueryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateQueryCommand request, CancellationToken cancellationToken)
        {
            var query = new Query
            {
                Body = request.Body,
                TodoId = request.TodoId,
                UserId = request.UserId,
                FileName = request.FileName,
                FilePath = request.FilePath
            };
            await _unitOfWork.Query.AddAsync(query);
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.TodoId);
            var user = await _unitOfWork.User.GetByIdAsync(request.UserId);
            if (todo.UserId != request.UserId)
            {
                var notification = new Notification
                {
                    Title = "New Query Posted",
                    Message = $"{user.FullName} has a query you to the task: {todo.Title} in workspace {todo.Project.Name}.",
                    RedirectLink = RedirectLink.Todo,
                    UserId = todo.UserId.Value,
                    IsRead = false,
                    CreatedTime = DateTime.Now,
                    RedirectId = todo.Id
                };
                await _unitOfWork.Notification.AddAsync(notification);
            }
            if (todo.CreatorId != request.UserId)
            {
                var notification = new Notification
                {
                    Title = "New Query Posted",
                    Message = $"{user.FullName} posted a new query on the task '{todo.Title}' in workspace '{todo.Project.Name}'.",
                    RedirectLink = RedirectLink.Todo,
                    UserId = todo.CreatorId,
                    IsRead = false,
                    CreatedTime = DateTime.Now,
                    RedirectId = todo.Id
                };
                await _unitOfWork.Notification.AddAsync(notification);
            }
            await _unitOfWork.SaveAsync();
            return query.TodoId;
        }
    }
}
