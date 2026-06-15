using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record TodoDto(int Id, string Title, string Description, Status Status, int CreatedBy, string CreatorName, int? UserId, string? UserName, string? FilePath, string? FileName, int ProjectId, string ProjectName, ICollection<QueryDto> QueryList);
}
