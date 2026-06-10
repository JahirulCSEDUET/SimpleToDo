using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Interfaces;

namespace SimpleToDo.Application.Services
{
    public class ToDoServices : IToDoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToDoServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Todo> AddAsync(Todo item)
        {
            await _unitOfWork.Todo.AddAsync(item);
            await _unitOfWork.SaveAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _unitOfWork.Todo.GetByIdAsync(id);
            if(item==null)
            {
                return false;
            }
            _unitOfWork.Todo.Delete(item);
            return await _unitOfWork.SaveAsync()>0;
        }

        public async Task<IReadOnlyList<Todo>> GetAllAsync()
        {
            return await _unitOfWork.Todo.GetAllAsync();
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _unitOfWork.Todo.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Todo>> GetByUserIdAsync(int userId, bool isArchived)
        {
            return _unitOfWork.Todo.Query()
                .Where(t=>t.UserId==userId && t.IsArchived==isArchived).ToList();
        }

        public async Task UpdateStatus(int id, string status)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(id);
            if (status == Status.Processing.ToString())
            {
                todo.Status = Status.Processing;
            }
            else if (status == Status.Completed.ToString())
            {
                todo.Status = Status.Completed;
            }
            else if(status == Status.Pending.ToString())
            {
                todo.Status = Status.Pending;
            }
            _unitOfWork.Todo.Update(todo);
            await _unitOfWork.SaveAsync();
        }
        public async Task<bool> ArchiveUnarchivedAsync(int id)
        {
            var todo = await _unitOfWork.Todo.GetByIdAsync(id);
            if(todo.IsArchived == true)
            {
                todo.IsArchived = false;
            }
            else
            {
                todo.IsArchived = true;
            }
            _unitOfWork.Todo.Update(todo);
            return await _unitOfWork.SaveAsync()>0;
        }

        public async Task UpdateAsync(Todo item)
        {
            _unitOfWork.Todo.Update(item);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IReadOnlyList<Todo>> GetByUserIdWithProjectAsync(int userId, bool isArchived)
        {
            return await _unitOfWork.Todo.GetByUserIdWithProjectAsync(userId, isArchived);
        }
    }
}
