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
        public string Description { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }
        public bool IsArchived { get; set; } 
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<Query> Queries { get; set; } = new List<Query>();
    }
}
