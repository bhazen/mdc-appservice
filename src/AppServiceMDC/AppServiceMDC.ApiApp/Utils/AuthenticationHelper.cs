using System;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Constants = AppServiceMDC.ApiApp.Utils.Contants;

namespace AppServiceMDC.ApiApp.Utils
{
    internal class AuthenticationHelper
    {
        public static ActiveDirectoryClient GetActiveDirectoryClient()
        {
            var serviceRoot = new Uri(Constants.ServiceRootUrl);
            var activeDirectoryClient = new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsync());
            return activeDirectoryClient;
        }

        private static async Task<string> AcquireTokenAsync()
        {
            var authContext = new AuthenticationContext(Constants.AuthString);
            var clientCredentails = new ClientCredential(Constants.ClientId, Constants.ClientSecret);
            var authResult = await authContext.AcquireTokenAsync(Constants.ResAzureGraphApi, clientCredentails);

            return authResult.AccessToken;
        }
    }
}