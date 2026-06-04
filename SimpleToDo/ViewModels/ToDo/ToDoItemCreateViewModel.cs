using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.ViewModels.ToDo
{
    public class ToDoItemCreateViewModel
    {
        public string Title { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }
}
