using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
