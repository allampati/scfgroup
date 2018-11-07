using CheckBoxList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class Communication
    {
        public List<SmartDevice> SelectedDevices { get; set; }
    }
}