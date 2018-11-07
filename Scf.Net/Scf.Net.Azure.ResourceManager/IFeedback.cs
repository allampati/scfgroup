using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Azure.ResourceManager
{
    public interface IFeedback
    {
        void OnTaskComplete(object info);
        void OnTaskError(string msg);
        void OnTaskException(Exception exception);
    }
}
