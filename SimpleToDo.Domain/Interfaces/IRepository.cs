
namespace SimpleToDo.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T item);
        void Delete(T item);
        Task<T> GetByIdAsync(int id);
        void Update(T item);
        IQueryable<T> Query();
        Task<IReadOnlyList<T>> GetAllAsync();
        
    }
}
