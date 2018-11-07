using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Azure.ResourceManager
{
    public class ResourceInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public string ProvisioningState { get; set; }
        public string RegionName { get; set; }        
        public List<KeyValuePair<string, string>> Tags { get; set; }
    }
}
