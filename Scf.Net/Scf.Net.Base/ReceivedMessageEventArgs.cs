using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Base
{
    public class ReceivedMessageEventArgs : System.EventArgs
    {
        public object Message { get; set; }
        public ReceivedMessageEventArgs(object message)
        {
            Message = message;
        }
    }
}
