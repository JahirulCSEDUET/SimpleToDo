using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Web.ViewModels.Project
{
    public class ProjectListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    }
}
