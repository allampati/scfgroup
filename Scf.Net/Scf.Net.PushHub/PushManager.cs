using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scf.Net.PushHub
{
    public static class PushManager
    {
        private static NotificationHubClient _client = null;
        private static BlockingCollection<PushMessage> msgSendQueue = new BlockingCollection<PushMessage>();
        private static CancellationTokenSource _ctsSend = new CancellationTokenSource();

        private static string _hubName = "";
        private static string _hubConnectionString = "";

        public static bool Start(string hubName, string connectionString)
        {
            _hubName = hubName;
            _hubConnectionString = connectionString;

            try
            {
                _client = NotificationHubClient.CreateClientFromConnectionString(_hubConnectionString, _hubName);
            }
            catch (Exception ex)
            {

            }

            Task.Run(async () =>
            {
                await PushMessagesToHub(_ctsSend.Token);
            });

            return true;
        }

        public static void Stop()
        {
            _ctsSend.Cancel();
        }

        public static void SendMessage(PushMessage msg)
        {
            msgSendQueue.Add(msg);
        }

        private static async Task PushMessagesToHub(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {

                    PushMessage msgObject = msgSendQueue.Take();

                    if (msgObject == null)
                        continue;

                    string message = msgObject.ToString();
                    switch (msgObject.Type)
                    {
                        case PushMessage.PushType.Android:
                            await _client.SendGcmNativeNotificationAsync(message);
                            break;
                        case PushMessage.PushType.Apple:
                            await _client.SendAppleNativeNotificationAsync(message);
                            break;
                        case PushMessage.PushType.Windows:
                            await _client.SendWindowsNativeNotificationAsync(message);
                            break;
                        case PushMessage.PushType.WindowsPhone:
                            await _client.SendWindowsNativeNotificationAsync(message);
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
