using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.DTOs
{
    public class ProjectAnalyticsDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public int TotalTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int UnassignedTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public double ProgressPercentage => TotalTasks == 0 ? 0 : Math.Round((double)CompletedTasks / TotalTasks * 100, 1);

        public List<AssigneeMemberDto> AssigneeConditions { get; set; } = new();
    }

    public class AssigneeMemberDto
    {
        public string UserName { get; set; } = null!;
        public int TodoCount { get; set; }
        public int InProgressCount { get; set; }
        public int DoneCount { get; set; }
        public int TotalAssigned => TodoCount + InProgressCount + DoneCount;
    }
}
