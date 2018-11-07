using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScfAzureResourceDeployment.Models
{
    public class Resource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public IDictionary<string, string> Tags { get; set; }
        public string ConnectionString { get; set; }
    }
}