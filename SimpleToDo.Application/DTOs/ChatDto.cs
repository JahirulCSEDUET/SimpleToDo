using SimpleToDo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class ChatDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public ICollection<string> UserIds { get; set; } = new List<string>();
    }
    
}
