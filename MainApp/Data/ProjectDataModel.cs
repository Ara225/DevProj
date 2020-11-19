using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace ProjectManager.Data
{
    [DynamoDBTable("ProjectIdeasDBProd")]
    public class ProjectDataModel
    {
        [DynamoDBRangeKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UpVotes { get; set; }

        public string Author { get; set; }
        public string Source { get; set; } = "";

        [DynamoDBHashKey]
        public ProjectType Type { get; set; }

        public bool isPublic { get; set; }
    }
}