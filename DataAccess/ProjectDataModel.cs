using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    [DynamoDBTable("DevProjProjectsTable")]
    public class ProjectDataModel
    {
        public ProjectDataModel(string ProjectName, string ProjectDescription, string ProjectOwnerId, bool ProjectIsPrivate)
        {
            if (ProjectName == null || ProjectDescription == null || ProjectOwnerId == null || isPrivate == null)
            {
                throw new ArgumentNullException();
            }

            Name = ProjectName;
            NormalizedName = ProjectName.ToUpper();
            Description = ProjectDescription;
            OwnerId = ProjectOwnerId;
            isPrivate = ProjectIsPrivate;
            CreatedOn = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        /// <summary>
        /// Id of user who owns the project
        /// </summary>
        public string OwnerId { get; set; }

        public string LinkedRepositoryURL { get; set; }
        
        public string CreatedOn { get; set; }

        public string Description { get; set; }

        public bool isPrivate { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
