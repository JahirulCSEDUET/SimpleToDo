using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.ViewModels.ToDo
{
    public class ToDoItemListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
    }
}
