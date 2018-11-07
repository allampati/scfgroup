using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class Parameter : BaseTable
    {
        public enum ValueType { Byte, Short, Integer, Long, Float, Double, DateTime, String };
        public enum DbLogType { None, All, AlertsOnly };
        public enum IntervalType { Second, Minute, Hour, Day};

        public string Name { get; set; }

        public string Units { get; set; }

        public string Serial { get; set; }

        public ValueType Type { get; set; }

        public DbLogType LogToDatabase { get; set; }

        public int MonitoringInterval { get; set; }

        public IntervalType MonitoringIntervalType { get; set; }

        public string Description { get; set; }

        public List<Alarm> Alarms { get; set; }

        public bool Completed { get; set; }
    }
}