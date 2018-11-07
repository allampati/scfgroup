using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Base
{
    public interface IEventReceiver
    {
        bool OnMessage(object sender, ReceivedMessageEventArgs args);
    }
}
