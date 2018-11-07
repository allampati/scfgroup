using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.PushHub
{
    public class PushMessage
    {
        public enum PushType { Unknown, Android, Apple, Windows, WindowsPhone, SimpleHub };

        public string Id { get; set; }
        public PushType Type { get; set; }
        public string Message { get; set; }
        public bool IsBroadcast { get; set; }
        public string DeviceId { get; set; }

        private static int msgCounter = 1;

        public PushMessage()
        {
            Id = msgCounter.ToString();
            msgCounter++;
        }

        public override string ToString()
        {
            string pushmessage = Message;

            switch (Type)
            {
                case PushType.Android:
                    pushmessage = "{ \"data\":{ \"message\":\"" + Message + "\"} }";
                    break;
                case PushType.Apple:
                    pushmessage = "{\"aps\":{\"alert\":\"" + Message + "\"}}";
                    break;
                case PushType.Windows:
                    pushmessage = "<? xml version = \"1.0\" encoding = \"utf-8\" ?><toast><visual><binding template = \"ToastText01\"><text id = \"" + Id + "\">" + Message + "</text></binding></visual></toast>";
                    break;
                case PushType.WindowsPhone:
                    pushmessage = "<? xml version = \"1.0\" encoding = \"utf-8\" ?><wp : Notification xmlns: wp = \"WPNotification\"><wp:Toast><wp:Text1> NotificationHub </wp:Text1>< wp:Text2>" + Message + "</wp:Text2></wp:Toast></wp:Notification>";
                    break;
            }

            return pushmessage;
        }
    }
}
