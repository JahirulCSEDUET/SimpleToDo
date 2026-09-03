using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Features.Chats.Queries
{
    public class GetChatByIdQuery: IRequest<ChatDto>
    {
        public int Id { get; set; }
    }
    public class GetChatByIdQueryHandler : IRequestHandler<GetChatByIdQuery, ChatDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChatByIdQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ChatDto> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
        {
            
            var chat = await _unitOfWork.Chats.GetByIdAsync(request.Id);

            var chatdto = new ChatDto
            {
                Id = chat.Id,
                ProjectId = chat.ProjectId,
                UserIds = chat.Project.ProjectMembers.Select(x => x.User.UserId).ToList()
            };
            return chatdto;
        }
    }
}
