using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Text;

namespace SimpleToDo.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Project> AddAsync(Project project)
        {
            await _unitOfWork.Project.AddAsync(project);
            await _unitOfWork.SaveAsync();
            return project;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _unitOfWork.Project.GetByIdAsync(id);
            if(project == null) {
                return false;
            }
            _unitOfWork.Project.Delete(project);
            return await _unitOfWork.SaveAsync()>0;
        }

        

        public async Task<Project> GetByIdAsync(int id)
        {
            return await _unitOfWork.Project.GetByIdAsync(id);
        }

        public async Task<Project> GetByIdWithMemberAndTodoAsync(int projectId)
        {
            return await _unitOfWork.Project.GetByIdWithMemberAndTodoAsync(projectId);

        }

        public async Task<IReadOnlyList<Project>> GetByMemberIdAsync(int userId)
        {
            return await _unitOfWork.Project.GetByMemberIdAsync(userId);
        }

        public async Task UpdateAsync(Project project)
        {
            _unitOfWork.Project.Update(project);
            await _unitOfWork.SaveAsync();
        }
    }
}
