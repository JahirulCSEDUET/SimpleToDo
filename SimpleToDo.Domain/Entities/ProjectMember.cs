using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class ProjectMember
    {
        public int Id { get; set; }
        public int ProjectId {  get; set; }
        public Project? Project { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public Role Role { get; set;  }

    }
}
