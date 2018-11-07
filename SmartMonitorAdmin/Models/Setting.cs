using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class Setting : BaseTable
    {
        public enum ValueType { String, Integer, Double };

        public string Name { get; set; }

        public string Value { get; set; }

        public string Units { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }
    }
}