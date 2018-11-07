using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMonitorWebJob
{
    public class SmartDevice : BaseTable
    {
        public enum PlatformType { Android, Apple, UWP, Windows, Analog, Digital};

        public string Name { get; set; }

        public string UserId { get; set; }

        public string DeviceId { get; set; }

        public string DeviceKey { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public PlatformType Platform { get; set; }

        public bool GpsTracking { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Selected { get; set; }

        public bool Completed { get; set; }

        public SmartDevice()
        {
            Latitude = 0.0f;
            Longitude = 0.0f;
        }
    }
}