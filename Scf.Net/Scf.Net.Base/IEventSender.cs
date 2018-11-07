using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Base
{
    public interface IEventSender
    {
        void OnMessage(object sender, ReceivedMessageEventArgs args);
        void OnStatus(object sender, SendStatusEventArgs args);
    }
}
