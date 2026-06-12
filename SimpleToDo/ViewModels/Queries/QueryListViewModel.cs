using SimpleToDo.Domain.Entities;

namespace SimpleToDo.Web.ViewModels.Queries
{
    public class QueryListViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public string FileName {  get; set; }
        public string FilePath { get; set;}
        public string UserName { get; set; }
    }
}
