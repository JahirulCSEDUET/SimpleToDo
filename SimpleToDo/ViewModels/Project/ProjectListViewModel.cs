using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Web.ViewModels.Project
{
    public class ProjectListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentUserId { get; set; }
        public ICollection<ProjectMemberListViewModel> ProjectMembers { get; set; } = new List<ProjectMemberListViewModel>();
    }
}
