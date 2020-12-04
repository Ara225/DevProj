using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBDataAccess
{
    [DynamoDBTable("DevProjUsersTable")]
    public class DynamoDBUser
    {
        //
        // Summary:
        //     Initializes a new instance of the class
        public DynamoDBUser()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
            ConcurrencyStamp = Guid.NewGuid().ToString();
            LoginProviders = new List<string>();
            LoginProviderKeys = new List<string>();
            LoginProviderDisplayNames = new List<string>();
        }

        //
        // Summary:
        //     Initializes a new instance of the class
        //
        // Parameters:
        //   userName:
        //     The user name.
        public DynamoDBUser(string userName) : this()
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
            NormalizedUserName = userName.ToUpper();
            CreatedOn = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        //
        // Summary:
        //     Gets or sets the date and time, in UTC, when any user lockout ends.
        //
        // Remarks:
        //     A value in the past means the user is not locked out.
        public string? LockoutEnd { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if two factor authentication is enabled for this
        //     user.
        //
        // Value:
        //     True if 2fa is enabled, otherwise false.
        [PersonalData]
        public bool TwoFactorEnabled { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their telephone address.
        //
        // Value:
        //     True if the telephone number has been confirmed, otherwise false.
        [PersonalData]
        public bool PhoneNumberConfirmed { get; set; }

        //
        // Summary:
        //     Gets or sets a telephone number for the user.
        [ProtectedPersonalData]
        public string PhoneNumber { get; set; }

        //
        // Summary:
        //     A random value that must change whenever a user is persisted to the store
        public string ConcurrencyStamp { get; set; }

        //
        // Summary:
        //     A random value that must change whenever a users credentials change (password
        //     changed, login removed)
        public string SecurityStamp { get; set; }
        //
        // Summary:
        //     Gets or sets a salted and hashed representation of the password for this user.
        public string PasswordHash { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their email address.
        //
        // Value:
        //     True if the email address has been confirmed, otherwise false.
        [PersonalData]
        public bool EmailConfirmed { get; set; }

        //
        // Summary:
        //     Gets or sets the normalized email address for this user.
        [ProtectedPersonalData]
        public string NormalizedEmail { get; set; }

        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        public string Email { get; set; }

        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        public string GitHubEmail { get; set; }

        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        public string NormalizedGitHubEmail { get; set; }

        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        public string PendingEmail { get; set; }

        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        public string NormalizedPendingEmail { get; set; }

        //
        // Summary:
        //     Gets or sets the normalized user name for this user.
        [ProtectedPersonalData]
        public string NormalizedUserName { get; set; }

        //
        // Summary:
        //     Gets or sets the user name for this user.
        [ProtectedPersonalData]
        public string UserName { get; set; }

        //
        // Summary:
        //     Gets or sets the primary key for this user.
        [PersonalData]
        [DynamoDBHashKey]
        public string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the date the user was created
        public string CreatedOn { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if the user could be locked out.
        //
        // Value:
        //     True if the user could be locked out, otherwise false.
        public bool LockoutEnabled { get; set; }

        //
        // Summary:
        //     Gets or sets the number of failed login attempts for the current user.
        public int AccessFailedCount { get; set; }

        //
        // Summary:
        //     The login providers used by the current user
        public List<string> LoginProviders { get; set; }

        //
        // Summary:
        //     The login provider keys linked to the current user
        public List<string> LoginProviderKeys { get; set; }

        //
        // Summary:
        //     The display name of the login providers used by the current user
        public List<string> LoginProviderDisplayNames { get; set; }

        //
        // Summary:
        //     URL to profile image
        public string ProfileImageURL { get; set; }

        //
        // Summary:
        //     Returns the username for this user.
        public override string ToString()
        {
            return UserName;
        }
    }
}
