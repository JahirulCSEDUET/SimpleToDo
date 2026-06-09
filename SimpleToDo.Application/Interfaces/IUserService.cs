using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        User GetByUserId(string userId);
    }
}
