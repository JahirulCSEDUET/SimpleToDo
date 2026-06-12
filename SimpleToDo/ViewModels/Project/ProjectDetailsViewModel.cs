using SimpleToDo.Domain.Entities;
using SimpleToDo.Web.ViewModels.ToDo;

namespace SimpleToDo.Web.ViewModels.Project
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DetailsViewerRole { get; set; }
        public ICollection<ProjectMemberListViewModel> ProjectMemberList { get; set; } = new List<ProjectMemberListViewModel>();
        public ICollection<ToDoItemListViewModel> TodoList { get; set; } = new List<ToDoItemListViewModel>();
    }
}
