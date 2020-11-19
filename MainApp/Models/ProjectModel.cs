using System;
using System.Collections.Generic;

namespace ProjectManager.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Source { get; set; } = "";

        public int Type { get; set; }
    }
}