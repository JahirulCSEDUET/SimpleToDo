using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Services
{
    public class ToDoServices : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoServices(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDoItem> AddAsync(ToDoItem item)
        {
            await _toDoRepository.AddAsync(item);
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _toDoRepository.GetByIdAsync(id);
            if(item==null)
            {
                return false;
            }
            return await _toDoRepository.DeleteAsync(item);
        }

        public async Task<IReadOnlyList<ToDoItem>> GetAllASync()
        {
            return await _toDoRepository.GetAllASync();
        }

        public async Task<ToDoItem> GetByIdAsync(int id)
        {
            return await _toDoRepository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<ToDoItem>> GetByUserIdAsync(string userId, bool isArchived)
        {
            return await _toDoRepository.GetByUserIdAsync(userId, isArchived);
        }

        public async Task UpdateStatus(int id, string status)
        {
            var todo = await _toDoRepository.GetByIdAsync(id);
            if (status == Status.Processing.ToString())
            {
                todo.Status = Status.Processing;
            }
            else if (status == Status.Completed.ToString())
            {
                todo.Status = Status.Completed;
            }
            await _toDoRepository.UpdateAsync(todo);
        }
        public async Task<bool> ArchiveUnarchivedAsync(int id)
        {
            var todo = await _toDoRepository.GetByIdAsync(id);
            if(todo.IsArchived == true)
            {
                todo.IsArchived = false;
            }
            else
            {
                todo.IsArchived = true;
            }
            await _toDoRepository.UpdateAsync(todo);
            return true;
        }
    }
}
