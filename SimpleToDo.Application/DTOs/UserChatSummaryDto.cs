using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class UserChatSummaryDto
    {
        public int ChatId { get; set; }
        public int ProjectId { get; set; }
        public string ChatName { get; set; } = null!;
        public int UnreadCount { get; set; }
        public string? LastMessage { get; set; }
        public string? LastMessageTime { get; set; }
    }
}
