using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.ViewModels.Project
{
    public class ProjectMemberListViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
