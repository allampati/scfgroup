using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Base
{
    public class SendStatusEventArgs : System.EventArgs
    {
        public enum StatusCode
        {
            Success = 0,
            Expired = 1,
            DeliveryCountExceeded = 2,
            Rejected = 3,
            Purged = 4
        }
        public string OriginalMessageId { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public StatusCode Status { get; set; }
    }
}
