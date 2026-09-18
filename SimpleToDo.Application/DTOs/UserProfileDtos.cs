using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    namespace SimpleToDo.Application.DTOs
    {
        public class UserProfileDto
        {
            public int Id {  get; set; }
            public string UserId { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int TotalWorkspaces { get; set; }
            public int TotalAssignedTasks { get; set; }
            public int CompletedTasksCount { get; set; }
            public int PendingTasksCount { get; set; }
        }

        public class UpdateProfileDto
        {
            public string FullName { get; set; } = string.Empty;
        }
    }
}
