using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScfAzureResourceDeployment.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public List<ResourceGroup> ResourceGroups { get; set; }
    }
}