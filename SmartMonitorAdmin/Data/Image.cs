using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorAdmin.Data
{
    public class Image
    {
        public Image(string name, string path, string desc)
        {
            Name = name;
            Path = path;
            Description = desc;
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
    }
}