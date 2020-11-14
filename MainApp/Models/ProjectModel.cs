using System;
using System.Collections.Generic;

namespace ProjectManager.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Source { get; set; } = "";

        public List<string> Steps { get; set; } = new List<string> {};

        public List<string> UserStories { get; set; } = new List<string> {};

        public List<string> Examples { get; set; } = new List<string> {};

        public int Type { get; set; }
    }
}