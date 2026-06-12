using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.ViewModels.ToDo
{
    public class ToDoItemCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? File { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
