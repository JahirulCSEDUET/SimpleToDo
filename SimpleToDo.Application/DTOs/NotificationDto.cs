using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public record NotificationDto(int Id, string Title, string Message, bool IsRead, string RedirectLink, int TimeAgo, int RedirectId);
}
