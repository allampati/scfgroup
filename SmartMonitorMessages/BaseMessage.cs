using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMonitorMessages
{
    public abstract partial class BaseMessage
    {
        private static int msgCounter = 1;

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageClass { get { return this.GetType().Name; } }

        public BaseMessage()
        {
            Id = msgCounter.ToString();
            msgCounter++;
        }
    }
}
