using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record ProjectDto(
        int Id, 
        string Name, 
        ICollection<ProjectMemberDto> ProjectMembers, 
        ICollection<TodoDto> TodoList
        );
}
