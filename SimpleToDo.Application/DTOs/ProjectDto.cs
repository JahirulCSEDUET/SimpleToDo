using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record ProjectDto(int Id, string Name, string DetailsViewerRole, ICollection<ProjectMemberDto> ProjectMemberList, ICollection<TodoDto> TodoList);
}
