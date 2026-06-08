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
        public Project Project { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public Role Role { get; set;  }

    }
}
