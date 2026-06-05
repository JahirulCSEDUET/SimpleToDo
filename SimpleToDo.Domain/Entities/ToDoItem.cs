using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
        public bool IsArchived { get; set; } 
    }
}
