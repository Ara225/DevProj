using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBUserStore
{
    public class InMemoryUserDataAccess
    {
        private List<DynamoDBUser> _users;
        public InMemoryUserDataAccess()
        {
            _users = new List<DynamoDBUser>();
        }
        public bool CreateUser(DynamoDBUser user)
        {
            _users.Add(user);
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

        public bool Update(DynamoDBUser user)
        {
            // Since get user gets the user from the same in-memory list,
            // the user parameter is the same as the object in the list, so nothing needs to be updated here.
            return true;
        }
    }
}