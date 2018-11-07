using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.MongoDb
{
    public class DbConnectionInfo
    {
        public string EndpointId { get; set; }
        public string AuthKey { get; set; }
        public string DatabaseId { get; set; }

        public DbConnectionInfo(string endPoint, string authKey, string databaseId)
        {
            EndpointId = endPoint;
            AuthKey = authKey;
            DatabaseId = databaseId;
        }
    }
}
