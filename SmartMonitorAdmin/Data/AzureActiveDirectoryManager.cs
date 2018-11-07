
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SmartMonitorAdmin.Data
{
    public class AzureActiveDirectoryManager
    {
        private static AzureActiveDirectoryManager manager = new AzureActiveDirectoryManager();

        public static AzureActiveDirectoryManager Instance { get { return manager; } }

        public void Initialize()
        {
            //Task.Run(async () =>
            //{
            //    await AddUser();
            //}).Wait();
        }

        public async Task AddUser()
        {
            string graphResourceId = "https://graph.windows.net";
            string tenantId = "msoppadandiyahoo.onmicrosoft.com";
            AuthenticationContext authContext = new AuthenticationContext("https://login.microsoftonline.com/msoppadandiyahoo.onmicrosoft.com");
            ClientCredential credential = new ClientCredential("{clientId}", "{secret}");
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            AuthenticationResult result = await authContext.AcquireTokenSilentAsync(graphResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            var accessToken = result.AccessToken;


            Uri servicePointUri = new Uri(graphResourceId);
            Uri serviceRoot = new Uri(servicePointUri, tenantId);

            ActiveDirectoryClient graphClient = new ActiveDirectoryClient(serviceRoot, async () => await Task.FromResult(accessToken));

            var user = new User();
            user.AccountEnabled = true;
            user.DisplayName = "testName";
            user.UserPrincipalName = "testName@xxx.onmicrosoft.com";
            user.MailNickname = "testName";
            user.UsageLocation = "US";
            user.PasswordProfile = new PasswordProfile
            {
                Password = "xxxxxx",
                ForceChangePasswordNextLogin = true
            };

            await graphClient.Users.AddUserAsync(user);
        }
    }
}