using Scf.Net.Base;
using Scf.Net.BlobStorage;
using Scf.Net.IotHub;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMonitorWebJob
{
    public class GpsHubSystem : IEventFeedback, IEventReceiver
    {
        private static GpsHubSystem _system = new GpsHubSystem();

        public static GpsHubSystem Instance { get { return _system; } }

        private List<SmartDevice> _gpsDevices = null;

        private bool Terminated { get; set; }
        private string hubConnectionString = "HostName=scfhub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=fjurTdfqViHbqnzw2chDN4m86NHLuNkpeGiMWdobtA8=";
        private string blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=scfstorage;AccountKey=KD/SR52ZjNepbQ/37d+t1aATlt+3rriafGMWbcFzIWIauLLILQ3QZhjaS7LS8iYPJZl3ER1tK10mA8vK//XwDQ==;EndpointSuffix=core.windows.net";
        private Receiver _iotHubReceiver = null;

        private BlobQueue _gpsQueue = null;

        public void Initialize()
        {
            Terminated = false;

            _gpsQueue = new BlobQueue("gpsmessages", blobConnectionString, this, this);
            _gpsQueue.Initialize();

            _iotHubReceiver = new Receiver(hubConnectionString, this);

            _iotHubReceiver.Initialize(this);

            BlobTable<SmartDevice>.Initialize(blobConnectionString);

            _gpsDevices = BlobTable<SmartDevice>.GetAllItems();
        }

        public void Terminate()
        {
            Terminated = true;

            _iotHubReceiver.Terminate();

            _gpsQueue.Terminate();

            BlobTable<SmartDevice>.Terminate();
        }


        private void ProcessGpsMessage(GpsMessage gpsMsg)
        {
            if (gpsMsg == null)
                return;

            try
            {
                SmartDevice device = _gpsDevices.Find(x => x.DeviceId == gpsMsg.DeviceId);
                if (device != null)
                {
                    device.Latitude = gpsMsg.Latitude;
                    device.Longitude = gpsMsg.Longitude;
                    BlobTable<SmartDevice>.Update(device);
                }

#if DEBUG
                Console.WriteLine(Scf.Net.IotHub.MessageHelper.ToString(gpsMsg));
#endif

                _gpsQueue.SendMessage(Scf.Net.IotHub.MessageHelper.ToString(gpsMsg));
            }
            catch (Exception ex)
            {

            }
        }

        public bool OnMessage(object sender, ReceivedMessageEventArgs args)
        {
            BaseMessage msg = args.Message as BaseMessage;

            if (msg == null)
                return false;

            switch(msg.MessageClass)
            {
                case "GpsMessage":
                    ProcessGpsMessage(msg as GpsMessage);
                    break;
            }

            return true;
        }

        public void OnComplete(object sender, TaskEventArgs args)
        {

        }

        public void OnError(object sender, InternalErrorEventArgs args)
        {

        }

        public void OnException(object sender, Exception exception)
        {

        }

        public void OnSendStatus(object sender, SendStatusEventArgs args)
        {
        }
    }
}
