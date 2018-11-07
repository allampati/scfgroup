using Microsoft.Azure.Devices;
using Microsoft.ServiceBus.Messaging;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SmartMonitorAdmin.Data
{
    public class IoTHubEventManager
    {
        private static IoTHubEventManager _manager = new IoTHubEventManager();

        public static IoTHubEventManager Instance { get { return _manager; } }

        static EventHubClient eventHubClient = null;
        static Queue<BaseMessage> msgReceiveQueue = new Queue<BaseMessage>();

        static ServiceClient serviceClient;
        static Queue<BaseMessage> msgSendQueue = new Queue<BaseMessage>();

        int msgCounter = 1;

        public void Initialize()
        {
            string connectionstring = ConfigurationManager.AppSettings["IoTHubConnectionString"];
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionstring, "messages/events");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionstring);

            Task.Run(async () =>
            {
                await ReadMessages();
            });

            Task.Run(async () =>
            {
                await SendCloudToDeviceMessageAsync();
            });
        }

        public List<BaseMessage> GetMessagesFromQueue()
        {
            if (msgReceiveQueue.Count <= 0)
                return null;

            List<BaseMessage> list = new List<BaseMessage>();

            while(msgReceiveQueue.Count > 0)
            {
                list.Add(msgReceiveQueue.Dequeue());
            }

            return list;
        }

        public async Task<bool> ReadMessages()
        {
            if (eventHubClient == null)
                return false;

            try
            {

                var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;
                CancellationTokenSource cts = new CancellationTokenSource();

                System.Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                var tasks = new List<Task>();
                foreach (string partition in d2cPartitions)
                {
                    tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
                }
                Task.WaitAll(tasks.ToArray());
            }
            catch(Exception ex)
            {
                
            }

            return true;
        }

        public void SendMessage(BaseMessage msg)
        {
            msg.Id = msgCounter.ToString();
            msgCounter++;
            msgSendQueue.Enqueue(msg);
        }

        private async static Task SendCloudToDeviceMessageAsync()
        {
            while (true)
            {
                while (msgSendQueue.Count > 0)
                {
                    BaseMessage msgObject = msgSendQueue.Dequeue();
                    var commandMessage = new Message(Encoding.ASCII.GetBytes(MessageHelper.ToString(msgObject)));
                    commandMessage.Ack = DeliveryAcknowledgement.Full;
                    await serviceClient.SendAsync(msgObject.DeviceId, commandMessage);
                }
            }
        }

        private async static void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);

            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null)
                    return;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                BaseMessage msg = MessageHelper.ToObject(data);
                if (msg != null)
                    msgReceiveQueue.Enqueue(msg);
            }
        }
    }
}