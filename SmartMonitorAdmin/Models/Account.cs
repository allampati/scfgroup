using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class Account : BaseTable
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool Completed { get; set; }

        public Account()
        {
            Id = "";
            CompanyName = "";
            Address = "";
            City = "";
            State = "";
            ZipCode = "";
            Phone = "";
            Email = "";
            Completed = false;
        }
    }
}