using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class ChatMessageDetailDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsMe { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string FormattedTime => CreatedDateTime.ToString("hh:mm tt");
    }
}
