using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Base
{
    public class TaskEventArgs : System.EventArgs
    {
        public object Task { get; set; }
        public TaskEventArgs(object task)
        {
            Task = task;
        }
    }
}
