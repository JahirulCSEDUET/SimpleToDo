using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Body {  get; set; }
        public int SenderId {  get; set; }
        public string SenderName {  get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set;  }
        public Chat Chat { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    }
}
