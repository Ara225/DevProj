using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;

namespace ProjectManager.Data
{
    public class DynamoDB
    {
        private static AmazonDynamoDBClient Client = new AmazonDynamoDBClient();
        private static DynamoDBContext context = new DynamoDBContext(Client);

        public static async Task<List<Project>> GetPublicProjects()
        {
            List<ScanCondition> ScanConditions = new List<ScanCondition>();
            ScanConditions.Add(new ScanCondition("isPublic", ScanOperator.Equal, true));
            AsyncSearch<Project> Projects = context.ScanAsync<Project>(ScanConditions);
            return await Projects.GetNextSetAsync();
        }

        public static async void SaveProject(Project project)
        {
            await context.SaveAsync(project);
        }

        public static async Task<Project> GetProject(string projectID)
        {
            return await context.LoadAsync<Project>(projectID);
        }

        public static async void DeleteProject(string projectID)
        {
            await context.DeleteAsync<Project>(projectID);
        }

        private static void FindRepliesPostedWithinTimePeriod()
        {
            string forumId = forumName + "#" + threadSubject;
            Console.WriteLine("\nFindRepliesPostedWithinTimePeriod: Printing result.....");

            DateTime startDate = DateTime.UtcNow - TimeSpan.FromDays(30);
            DateTime endDate = DateTime.UtcNow - TimeSpan.FromDays(1);

            IEnumerable<Reply> repliesInAPeriod = context.QueryAsync<Project>(forumId,
                                           QueryOperator.Between, startDate, endDate);
        }
    }
}