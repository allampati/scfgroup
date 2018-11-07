using Scf.Net.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMonitorSimulator
{
    public class MongoTestModel : MongoDocumentBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
