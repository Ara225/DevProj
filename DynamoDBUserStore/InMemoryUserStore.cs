using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDBUserStore
{
    public class InMemoryUserStore : IUserPasswordStore<DynamoDBUser>, IUserEmailStore<DynamoDBUser>, IUserLoginStore<DynamoDBUser>
    {
        private InMemoryUserDataAccess _dataAccess;
        public InMemoryUserStore(InMemoryUserDataAccess da)
        {
            _dataAccess = da;
        }

        public async Task AddLoginAsync(DynamoDBUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.LoginProviderDisplayNames.Add(login.ProviderDisplayName);
            user.LoginProviderKeys.Add(login.ProviderKey);
            user.LoginProviders.Add(login.LoginProvider);
        }

        public Task<IdentityResult> CreateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<IdentityResult>.Run(() =>
            {
                IdentityResult result = IdentityResult.Failed();
                bool createResult = _dataAccess.CreateUser(user);

                if (createResult)
                {
                    result = IdentityResult.Success;
                }

                return result;
            });
        }

        public async Task<IdentityResult> DeleteAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            _dataAccess.Delete(user);
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public Task<DynamoDBUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task<DynamoDBUser>.Run(() =>
            {
                return _dataAccess.GetByEmail(normalizedEmail);
            });
        }

        public Task<DynamoDBUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task<DynamoDBUser>.Run(() =>
            {
                return _dataAccess.GetUserById(userId);
            });
        }

        public async Task<DynamoDBUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return _dataAccess.GetUserByLogin(loginProvider, providerKey);
        }

        public Task<DynamoDBUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task<DynamoDBUser>.Run(() =>
            {
                return _dataAccess.GetUserByUsername(normalizedUserName);
            });
        }

        public Task<string> GetEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.Email;
            });
        }

        public Task<bool> GetEmailConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<bool>.Run(() =>
            {
                return user.EmailConfirmed;
            });
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            List<UserLoginInfo> UserLogins = new List<UserLoginInfo>();
            for (int i = 0; i < user.LoginProviders.Count; i++)
            {
                UserLogins.Add(new UserLoginInfo(user.LoginProviders[i], user.LoginProviderKeys[i], user.LoginProviderDisplayNames[i]));
            }
            return UserLogins;
        }

        public Task<string> GetNormalizedEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.NormalizedEmail;
            });
        }

        public Task<string> GetNormalizedUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.NormalizedUserName;
            });
        }

        public Task<string> GetPasswordHashAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() => { return user.PasswordHash; });
        }

        public Task<string> GetUserIdAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.Id;
            });
        }

        public Task<string> GetUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<string>.Run(() =>
            {
                return user.UserName;
            });
        }

        public Task<bool> HasPasswordAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<bool>.Run(() => { return true; });
        }

        public async Task RemoveLoginAsync(DynamoDBUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            for (int i = 0; i < user.LoginProviderKeys.Count; i++)
            {
                if (user.LoginProviderKeys[i] == providerKey)
                {
                    user.LoginProviderKeys.RemoveAt(i);
                    user.LoginProviderDisplayNames.RemoveAt(i);
                    user.LoginProviders.RemoveAt(i);
                    break;
                }
            }
        }

        public Task SetEmailAsync(DynamoDBUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.Email = email;
            });
        }

        public Task SetEmailConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.EmailConfirmed = confirmed;
            });
        }

        public Task SetNormalizedEmailAsync(DynamoDBUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.NormalizedEmail = normalizedEmail;
            });
        }

        public Task SetNormalizedUserNameAsync(DynamoDBUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.NormalizedUserName = normalizedName;
            });
        }

        public Task SetPasswordHashAsync(DynamoDBUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.PasswordHash = passwordHash; });
        }

        public Task SetUserNameAsync(DynamoDBUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.UserName = userName;
                user.NormalizedUserName = userName.ToUpper();
            });
        }

        public Task<IdentityResult> UpdateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task<IdentityResult>.Run(() =>
            {
                IdentityResult result = IdentityResult.Failed();
                bool updateResult = _dataAccess.Update(user);

                if (updateResult)
                {
                    result = IdentityResult.Success;
                }

                return result;
            });
        }
    }
}