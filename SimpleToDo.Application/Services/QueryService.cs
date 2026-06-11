using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Services
{
    public class QueryService : IQueryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QueryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Query> AddAsync(Query query)
        {
            await _unitOfWork.Query.AddAsync(query);
            await _unitOfWork.SaveAsync();
            return query;
        }

        public Task<IReadOnlyList<Query>> GetByUserIdAsync(int userId)
        {
            return _unitOfWork.Query.GetByUserIdAsync(userId);
        }
    }
}
