using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Azure.ResourceManager
{
    public static class AzureAccount
    {
        public static object Authenticate(string clientId, string clientSecret, string tenantId, string subscriptionId)
        {
            var credentials = new AzureCredentialsFactory().FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure.Authenticate(credentials).WithSubscription(subscriptionId);

            return azure;
        }

        public static List<string> Regions()
        {
            List<string> regions = new List<string>();

            foreach (Region item in Region.Values)
            {
                regions.Add(item.Name);
            }

            return regions;
        }
    }
}
