using SimpleToDo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message {  get; set; }
        public int UserId { get; set; }
        public User userId { get; set; }
        public bool IsRead { get; set; }
        public RedirectLink RedirectLink { get; set; }
        public DateTime CreatedTime { get; set; }
        public int RedirectId { get; set; }
    }
}
