using System;
using System.Numerics;

namespace ProjectManager.Data
{
    public class Project
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public BigInteger UpVotes { get; set; }

        public string Author { get; set; }

        public string? Source { get; set; }

        public Array? Steps { get; set; }

        public Array? UserStories { get; set; }

        public Array? Examples { get; set; }

        public ProjectType ProjectType { get; set; }

        public bool isPublic { get; set; }
    }
}