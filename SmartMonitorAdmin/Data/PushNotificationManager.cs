using Microsoft.Azure.NotificationHubs;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SmartMonitorAdmin.Data
{
    public class PushNotificationManager
    {
        private static PushNotificationManager _manager = new PushNotificationManager();

        public static PushNotificationManager Instance { get { return _manager; } }

        private static NotificationHubClient pushHub = null;
        static Queue<PushMessage> msgSendQueue = new Queue<PushMessage>();
        private static Object syncObject = new Object();

        public void Initialize()
        {
            string connectionstring = ConfigurationManager.AppSettings["PushHubConnectionString"];
            string hubname = ConfigurationManager.AppSettings["PushHubName"];

            pushHub = NotificationHubClient.CreateClientFromConnectionString(connectionstring, hubname);

            Task.Run(async () =>
            {
                await PushMessagesToHub();
            });
        }

        public void SendMessage(PushMessage msg)
        {
            lock (syncObject)
            {
                msgSendQueue.Enqueue(msg);
            }
        }

        private static async Task PushMessagesToHub()
        {
            while (true)
            {
                while (msgSendQueue.Count > 0)
                {
                    try
                    {

                        PushMessage msgObject = null;
                        lock (syncObject)
                        {
                            msgObject = msgSendQueue.Dequeue();
                        }
                        if (msgObject == null)
                            continue;

                        string message = msgObject.ToString();
                        switch (msgObject.Type)
                        {
                            case PushMessage.PushType.Android:
                                await pushHub.SendGcmNativeNotificationAsync(message);
                                break;
                            case PushMessage.PushType.Apple:
                                await pushHub.SendAppleNativeNotificationAsync(message);
                                break;
                            case PushMessage.PushType.Windows:
                                
                                await pushHub.SendWindowsNativeNotificationAsync(message);
                                break;
                            case PushMessage.PushType.WindowsPhone:
                                await pushHub.SendWindowsNativeNotificationAsync(message);
                                break;
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }
    }
}