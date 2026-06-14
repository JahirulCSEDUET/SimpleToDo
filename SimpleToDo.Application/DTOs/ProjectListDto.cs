using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record ProjectListDto(int Id, string Name, int CurrentUserId, ICollection<ProjectMemberDto> ProjectMembers);
}
