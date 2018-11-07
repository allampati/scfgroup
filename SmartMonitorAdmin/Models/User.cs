using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class User : BaseTable
    {
        public string UserId { get; set; }

        public string UserGroup { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }

        public string Ext { get; set; }

        public string MobilePhone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public bool Completed { get; set; }
    }
}