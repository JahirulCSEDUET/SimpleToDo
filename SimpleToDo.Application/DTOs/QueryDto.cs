using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record QueryDto(int Id, string Body, int UserId, string FileName, string FilePath, string UserName);
}
