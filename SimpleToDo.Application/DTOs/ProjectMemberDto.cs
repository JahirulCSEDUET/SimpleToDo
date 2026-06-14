using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record ProjectMemberDto(int Id ,int UserId ,string UserName,Role Role);
}
