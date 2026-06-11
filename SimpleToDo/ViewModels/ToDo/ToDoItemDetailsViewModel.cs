using SimpleToDo.Domain.Enums;
using SimpleToDo.Web.ViewModels.Queries;

namespace SimpleToDo.Web.ViewModels.ToDo
{
    public class ToDoItemDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ICollection<QueryListViewModel> QueryList { get; set; } = new List<QueryListViewModel>();
    }
}
