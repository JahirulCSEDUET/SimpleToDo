using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
        public bool IsArchived { get; set; } 
        public string CreatedBy { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
