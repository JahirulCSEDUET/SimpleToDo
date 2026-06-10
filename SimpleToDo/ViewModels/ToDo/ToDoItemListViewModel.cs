using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.ViewModels.ToDo
{
    public class ToDoItemListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public int CreatedBy { get; set; }
        public string? CreatorName { get; set; }
        public int? UserId { get; set; }
        public string?  UserName { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
    }
}
