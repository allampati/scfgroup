using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class Alarm : BaseTable
    {
        public enum AlarmType { Normal, Warning, Error, Critical };
        public enum OperatorType { EqualTo, LessThan, LessThanEqualTo, GreatThan, GreaterThanEqualTo, Contains };
        public enum ValType { Byte, Short, Integer, Long, Float, Double, DateTime, String};

        public string DeviceId { get; set; }
        public AlarmType Type { get; set; }
        public OperatorType Operator { get; set; }
        public string Value { get; set; }
        public ValType ValueType { get; set; }

        public Alarm()
        {
            Type = AlarmType.Normal;
            Operator = OperatorType.GreaterThanEqualTo;
            ValueType = ValType.Double;
            Value = "";
        }
    }
}