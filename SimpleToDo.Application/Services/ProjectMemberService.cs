using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectMemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectMember> AddAsync(ProjectMember projectMember)
        {
            await _unitOfWork.ProjectMember.AddAsync(projectMember);
            await _unitOfWork.SaveAsync();
            return projectMember;
        }

        public ProjectMember GetProjectMemberById(int memberId, int projectId)
        {
            var query = _unitOfWork.ProjectMember.Query().FirstOrDefault(pm=> pm.UserId == memberId && pm.ProjectId == projectId);
            return query;
        }
    }
}
