using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class UserChatsVm
    {
        public int TotalUnread { get; set; }
        public List<UserChatSummaryDto> Chats { get; set; } = new();
    }
}
