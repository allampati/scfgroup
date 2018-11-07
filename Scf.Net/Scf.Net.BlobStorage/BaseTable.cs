using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.BlobStorage
{
    public abstract class BaseTable
    {
        public string Id{get;set;}
        public string Partition { get; set; }
        public string Tag { get; set; }
        
        public BaseTable()
        {
            Partition = "Default";
            Id = Guid.NewGuid().ToString();
            Tag = "*";
        }
    }
}
