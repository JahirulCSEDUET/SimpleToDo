using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record TodoDto(
        int Id, 
        string Title, 
        string Description,
        string? FilePath,
        string? FileName,
        Status Status, 
        int CreatorId, 
        string CreatorName, 
        int? UserId, 
        string? UserName,          
        int ProjectId, 
        string ProjectName, 
        ICollection<QueryDto> QueryList
        );
}
