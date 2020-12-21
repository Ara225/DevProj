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
    [DynamoDBTable("DevProjGoalsTable")]
    public class GoalDataModel
    {
        public GoalDataModel()
        {

        }

        public GoalDataModel(string GoalName, string GoalDescription, string ProjectId, string GoalDueBy)
        {
            Id = Guid.NewGuid().ToString();
            Name = GoalName;
            NormalizedName = GoalName.ToUpper();
            Description = GoalDescription;
            ParentProjectId = ProjectId;
            DueBy = GoalDueBy;
        }

        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        /// <summary>
        /// Id of project the goal belongs to
        /// </summary>
        public string ParentProjectId { get; set; }

        public string DueBy { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
