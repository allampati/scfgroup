using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Base
{
    public interface IEventFeedback
    {
        void OnComplete(object sender, TaskEventArgs ar);
        void OnError(object sender, InternalErrorEventArgs args);
        void OnException(object sender, Exception ex);
    }
}
