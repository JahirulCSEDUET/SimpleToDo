using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class SendChatMessageResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Body { get; set; }
        public string? FormattedTime { get; set; }
        public string? SenderName { get; set; }
    }
}
