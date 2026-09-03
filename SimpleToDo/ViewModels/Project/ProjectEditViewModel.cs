using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.ViewModels.Project
{
    public class ProjectEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
