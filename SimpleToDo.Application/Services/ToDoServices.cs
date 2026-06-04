using SimpleToDo.Application.Interfaces;
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

        public async Task<bool> DeleteAsync(ToDoItem item)
        {
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

        public async Task<IReadOnlyList<ToDoItem>> GetByUserIdAsync(string userId)
        {
            return await _toDoRepository.GetByUserIdAsync(userId);
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
    }
}
