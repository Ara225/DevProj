using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDBDataAccess
{
    public class DynamoDBUserStore : IUserPasswordStore<DynamoDBUser>, IUserEmailStore<DynamoDBUser>, IUserLoginStore<DynamoDBUser>
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBUserStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        public async Task AddLoginAsync(DynamoDBUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.LoginProviderDisplayNames.Add(login.ProviderDisplayName);
            user.LoginProviderKeys.Add(login.ProviderKey);
            user.LoginProviders.Add(login.LoginProvider);
        }

        public async Task<IdentityResult> CreateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveItemToDB(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.DeleteItem(user, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public async Task<DynamoDBUser> FindByEmailAsync(string NormalizedEmail, CancellationToken cancellationToken)
        {
            if (NormalizedEmail == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();

            return await _dataAccess.GetUserByAttribute("NormalizedEmail", NormalizedEmail);
        }

        public async Task<DynamoDBUser> FindByIdAsync(string Id, CancellationToken cancellationToken)
        {
            if (Id == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserById(Id);
        }

        public async Task<DynamoDBUser> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancellationToken)
        {
            if (LoginProvider == null || ProviderKey == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserByLogin(LoginProvider, ProviderKey);
        }

        public async Task<DynamoDBUser> FindByNameAsync(string NormalizedUserName, CancellationToken cancellationToken)
        {
            return await _dataAccess.GetUserByAttribute("NormalizedUserName", NormalizedUserName);
        }

        public async Task<string> GetEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
             return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
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

        public async Task<string> GetNormalizedEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task<string> GetNormalizedUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedUserName;
        }

        public async Task<string> GetPasswordHashAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<string> GetUserIdAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }

        public async Task<bool> HasPasswordAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user.PasswordHash == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task RemoveLoginAsync(DynamoDBUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
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

        public async Task SetEmailAsync(DynamoDBUser user, string email, CancellationToken cancellationToken)
        {
             user.Email = email;
        }

        public async Task SetEmailConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
        }

        public async Task SetNormalizedEmailAsync(DynamoDBUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
        }

        public async Task SetNormalizedUserNameAsync(DynamoDBUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
        }

        public async Task SetPasswordHashAsync(DynamoDBUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PasswordHash = passwordHash;
        }

        public async Task SetUserNameAsync(DynamoDBUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            user.NormalizedUserName = userName.ToUpper();
        }

        public async Task<IdentityResult> UpdateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
             user.ConcurrencyStamp = Guid.NewGuid().ToString();
             IdentityResult Result = IdentityResult.Failed();
             bool UpdateResult = await _dataAccess.SaveItemToDB(user, cancellationToken);
             if (UpdateResult)
             {
                  Result = IdentityResult.Success;
             }
             return Result;
        }
    }
}