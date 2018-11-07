using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMonitorMessages
{
    public class GpsMessage : BaseMessage
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
