using Scf.Net.Base;
using Scf.Net.BlobStorage;
using SmartMonitorAdmin.Data;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SmartMonitorAdmin.Models
{
    public class GpsMap : IEventFeedback, IEventReceiver
    {
        private List<SmartDevice> _devices = new List<SmartDevice>();
        private BlobQueue _gpsQueue = null;

        public GpsMap()
        {
            _gpsQueue = new BlobQueue("gpsmessages", ConfigurationManager.AppSettings["BlobStorageConnectionString"], this, this);
            _gpsQueue.Initialize();

            Refresh();
        }

        public List<SmartDevice> Devices { get { return _devices; } }

        public void Refresh()
        {
            List<SmartDevice> items = BlobTable<SmartDevice>.GetAllItems();

            _devices.Clear();

            foreach (SmartDevice item in items)
            {
                if (item.GpsTracking)
                    _devices.Add(item);
            }
        }

        public bool OnMessage(object sender, ReceivedMessageEventArgs args)
        {
            GpsMessage msg = args.Message as GpsMessage;
            if(msg != null)
            {
                UpdateLocation(msg.DeviceId, msg.Latitude, msg.Longitude);
            }

            return true;
        }

        public void UpdateLocation(string deviceId, double lat, double lon)
        {
            SmartDevice device = _devices.Find(x => x.DeviceId == deviceId);
            if(device != null)
            {
                device.Latitude = lat;
                device.Longitude = lon;
            }
        }

        public void OnComplete(object sender, TaskEventArgs args)
        {
            
        }

        public void OnError(object sender, InternalErrorEventArgs args)
        {

        }

        public void OnException(object sender, Exception ex)
        {

        }
    }
}