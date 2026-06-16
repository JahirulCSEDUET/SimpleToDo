using AutoMapper;
using MediatR;
using SimpleToDo.Application.DTOs;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Features.Projects.Queries
{
    public record GetProjectsByUserIdQuery(int userId) : IRequest<IReadOnlyList<ProjectListDto>>;
    public class GetProjectByUserIdQueryHandler : IRequestHandler<GetProjectsByUserIdQuery, IReadOnlyList<ProjectListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProjectByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProjectListDto>> Handle(GetProjectsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Project.GetByUserIdAsync(request.userId);
            return _mapper.Map<IReadOnlyList<ProjectListDto>>(products);
        }
    }
}
