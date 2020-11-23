using DynamoDBUserStore;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevProjUnitTests
{
    [TestClass]
    public class UserStoreUnitTests
    {
        private InMemoryUserStore _store;
        private DynamoDBUser _user;

        [TestInitialize]
        public void Initialize()
        {
            _store = new InMemoryUserStore(new InMemoryUserDataAccess());
            _user = new DynamoDBUser("TestUser");
            IdentityResult CreateResult = _store.CreateAsync(_user, new CancellationToken()).Result;
            Assert.AreEqual(CreateResult, IdentityResult.Success);
        }

        [TestMethod]
        public async Task TestGetUser()
        {
            DynamoDBUser user = await _store.FindByIdAsync(_user.Id, new CancellationToken());
            Assert.AreEqual(user.Id, _user.Id);
        }
    }
}
