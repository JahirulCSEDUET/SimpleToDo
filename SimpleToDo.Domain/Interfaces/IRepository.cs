
namespace SimpleToDo.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T item);
        void Delete(T item);
        void Update(T item);
        IQueryable<T> Query();
        
    }
}
