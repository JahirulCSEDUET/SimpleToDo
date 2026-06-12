using SimpleToDo.Domain.Entities;
using SimpleToDo.Domain.Enums;

namespace SimpleToDo.Web.Views.Shared
{
    public class NotificationListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string RedirectLink { get; set; }
        public int TimeAgo { get; set; }
        public int RedirectId { get; set; }
    }
}
