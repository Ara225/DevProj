using System;

namespace ProjectManager.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string? Source { get; set; }

        public Array? Steps { get; set; }

        public Array? UserStories { get; set; }

        public Array? Examples { get; set; }

        public int Type { get; set; }
    }
}