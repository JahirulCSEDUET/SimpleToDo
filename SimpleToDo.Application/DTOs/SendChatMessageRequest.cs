using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class SendChatMessageRequest
    {
        public int ChatId { get; set; }
        public string Body { get; set; } = null!;
    }
}
