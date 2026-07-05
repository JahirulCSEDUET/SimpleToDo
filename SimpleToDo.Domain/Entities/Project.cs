using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Running;
        public bool IsDeleted { get; set; } = false;
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
