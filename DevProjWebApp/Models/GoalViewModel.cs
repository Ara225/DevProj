using System;
using System.Collections.Generic;

namespace DevProjWebApp.Models
{
    public class GoalViewModel
    {
        public string Name { get; set; }

        public string LinkedIssueURL { get; set; }
        
        public string Description { get; set; }

        public DateTime GoalDueBy { get; set; }
    }
}
