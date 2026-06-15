using MediatR;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Todos.Commands
{
    public record QuickAssignTodoCommand(int userId, int projectId, int todoId, int currentUserId, string currentUserName) : IRequest;
    public class QuickAssignTodoCommandHandler : IRequestHandler<QuickAssignTodoCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuickAssignTodoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(QuickAssignTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(request.todoId);
            if(todo == null)
            {
                throw new ArgumentNullException("Invalid todo id.");
            }
            var project = await _unitOfWork.Project.GetByIdAsync(request.projectId);
            if (project == null)
            {
                throw new ArgumentNullException("Invalid project id.");
            }
            
            todo.UserId = request.userId;
            _unitOfWork.Todo.Update(todo);
            var notification = new Notification
            {
                Title = "New Task Assigned",
                Message = $"{request.currentUserName} assigned you to the task: {todo.Title} in workspace {todo.Project.Name}.",
                RedirectLink = RedirectLink.Todo,
                UserId = request.userId,
                IsRead = false,
                CreatedTime = DateTime.Now,
                RedirectId = request.todoId
            };
            await _unitOfWork.Notification.AddAsync(notification);
            await _unitOfWork.SaveAsync();
        }
    }
}
