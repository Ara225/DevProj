using Amazon.DynamoDBv2.DataModel;
using System;
using System.Numerics;

namespace ProjectManager.Data
{
    [DynamoDBTable("ProjectIdeasDBProd")]
    public class Project
    {
        [DynamoDBRangeKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public BigInteger UpVotes { get; set; }

        public string Author { get; set; }

        public string? Source { get; set; }

        public Array? Steps { get; set; }

        public Array? UserStories { get; set; }

        public Array? Examples { get; set; }

        [DynamoDBHashKey]
        public ProjectType Type { get; set; }

        public bool isPublic { get; set; }
    }
}