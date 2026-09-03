using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record ProjectDto(
        int Id, 
        string Name, 
        ProjectStatus Status,
        bool IsDeleted,
        ICollection<ProjectMemberDto> ProjectMembers, 
        ICollection<TodoDto> TodoList
        );
}
