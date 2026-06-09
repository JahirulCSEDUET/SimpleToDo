using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Services
{
    
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> AddAsync(User user)
        {
            _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _unitOfWork.User.GetByIdAsync(id);

        }

        public User GetByUserId(string userId)
        {
            return _unitOfWork.User.Query().FirstOrDefault(i => i.UserId == userId);
        }
    }
}
