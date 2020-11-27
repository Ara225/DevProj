using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Threading;

namespace DynamoDBUserStore
{
    public class InMemoryUserDataAccess
    {
        private List<DynamoDBUser> _users;
        private AmazonDynamoDBClient _client;
        private DynamoDBContext _context;

        public InMemoryUserDataAccess(AmazonDynamoDBClient Client)
        {
            _client = Client;
            _context = new DynamoDBContext(Client); 
            _users = new List<DynamoDBUser>();
        }

        public async Task<bool> SaveItemToDB(dynamic user, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(user, cancellationToken);
            return true;
        }

        public DynamoDBUser GetUserById(string id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public DynamoDBUser GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.NormalizedEmail == email);
        }

        public DynamoDBUser GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.NormalizedUserName == username);
        }

        public string GetNormalizedUsername(DynamoDBUser user)
        {
            return user.NormalizedUserName;
        }

        public DynamoDBUser GetUserByLogin(string loginProvider, string providerKey)
        {
            return _users.FirstOrDefault(u => u.LoginProviders.Contains(loginProvider) && u.LoginProviderKeys.Contains(providerKey));
        }

        public bool Update(DynamoDBUser user)
        {
            // Since get user gets the user from the same in-memory list,
            // the user parameter is the same as the object in the list, so nothing needs to be updated here.
            return true;
        }

        public bool Delete(DynamoDBUser user)
        {
            _users.Remove(user);
            return true;
        }
    }
}