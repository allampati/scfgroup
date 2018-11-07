using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class UserGroup : BaseTable
    {
        public enum MembershipType { Assigned, Static, Dynamic};

        public string Name { get; set; }

        public string Description { get; set; }

        public MembershipType Type { get; set; }

        public bool Completed { get; set; }

        public UserGroup()
        {
            Type = MembershipType.Assigned;
        }
    }
}