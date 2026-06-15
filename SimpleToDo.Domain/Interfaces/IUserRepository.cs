using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Interfaces
{
    public interface IUserRepository :IRepository<User>
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUserIdAsync(string userId);
        
    }
}
