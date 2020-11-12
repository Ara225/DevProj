using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectManager.Models;
using System.Linq;

namespace ProjectManager.Data
{
    public static class DynamoDB
    {
        private static AmazonDynamoDBClient Client = new AmazonDynamoDBClient();
        private static DynamoDBContext context = new DynamoDBContext(Client);

        public static async Task<List<ProjectDataModel>> GetPublicProjects()
        {
            //TODO Implement pagination && filtering
            List<ScanCondition> ScanConditions = new List<ScanCondition>();
            ScanConditions.Add(new ScanCondition("isPublic", ScanOperator.Equal, true));
            AsyncSearch<ProjectDataModel> Projects = context.ScanAsync<ProjectDataModel>(ScanConditions);
            return await Projects.GetNextSetAsync();
        }

        public static async Task<bool> SaveProject(ProjectModel project)
        {
            ProjectDataModel projectData = new ProjectDataModel
            {
                Id = GetId(10),
                Name = project.Name,
                Description = project.Description,
                UpVotes = 0,
                Author = "A_USER",
                Source = project.Source,
                Steps = project.Steps,
                UserStories = project.UserStories,
                Examples = project.Examples,
                Type = (ProjectType)project.Type,
                isPublic = true
            };
            await context.SaveAsync(projectData);
            return true;
        }

        public static async Task<ProjectDataModel> GetProject(string projectID)
        {
            return await context.LoadAsync<ProjectDataModel>(projectID);
        }

        public static async Task<bool> DeleteProject(string projectID)
        {
            await context.DeleteAsync<ProjectDataModel>(projectID);
            return true;
        }

        public static async Task<List<ProjectDataModel>> QueryByType(ProjectType Type)
        {
            //TODO Implement pagination
            List<object> TypeList = new List<object>();
            TypeList.Add(Type);
            List<ScanCondition> ScanConditions = new List<ScanCondition>();
            ScanConditions.Add(new ScanCondition("isPublic", ScanOperator.Equal, true));
            AsyncSearch<ProjectDataModel> ProjectSearch = context.QueryAsync<ProjectDataModel>("Type", QueryOperator.Equal, TypeList,
                                                                             new DynamoDBOperationConfig { QueryFilter = ScanConditions });
            return await ProjectSearch.GetNextSetAsync();
        }
        // Code created with the help of Stack Overflow question
        // https://stackoverflow.com/questions/1122483/random-string-generator-returning-same-string
        // Question by PushCode https://stackoverflow.com/users/136271/pushcode
        // Answer by Zygimantas https://stackoverflow.com/users/106715/zygimantas
        public static string GetId(int length)
        {
            System.Byte[] seedBuffer = new System.Byte[4];
            using (var rngCryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(seedBuffer);
                System.String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                System.Random random = new System.Random(System.BitConverter.ToInt32(seedBuffer, 0));
                return new System.String(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}