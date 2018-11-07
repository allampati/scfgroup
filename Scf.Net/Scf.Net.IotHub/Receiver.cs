using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using System.Collections.Concurrent;
using Scf.Net.Base;

namespace Scf.Net.IotHub
{
    public class Receiver
    {
        static EventHubClient eventHubClient = null;
        private string _connectionString = "";
        private string _entityPath = "messages/events";

        private static IEventFeedback _feedback = null;
        private static IEventReceiver _receiver = null;

        static BlockingCollection<object> msgReceiveQueue = new BlockingCollection<object>();

        private static CancellationTokenSource _ctsNotify = new CancellationTokenSource();
        private static CancellationTokenSource _ctsRead = new CancellationTokenSource();

        public Receiver(string connectionString, IEventFeedback feedback)
        {
            _connectionString = connectionString;
            _feedback = feedback;
        }

        public bool Initialize(IEventReceiver receiver)
        {
            _receiver = receiver;

            return Initialize(_entityPath, _receiver);
        }

        public bool Initialize(string entityPath, IEventReceiver receiver)
        {
            _entityPath = entityPath;
            _receiver = receiver;

            eventHubClient = EventHubClient.CreateFromConnectionString(_connectionString, _entityPath);
            Task.Run(async () =>
            {
                await ReadMessages();
            });

            Task.Run(async () =>
            {
                await NotifyEvents();
            });

            return true;
        }

        public void Terminate()
        {
            _ctsRead.Cancel();
            _ctsNotify.Cancel();
        }

        public async Task NotifyEvents()
        {
            try
            {
                var tasks = new List<Task>();

                tasks.Add(NotifyHubEventAsync(_ctsNotify.Token));

                Task.WaitAll(tasks.ToArray());
            }
            catch(Exception ex)
            {

            }
        }

        private static async Task NotifyHubEventAsync(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    object msg = msgReceiveQueue.Take();
                    if (msg != null && _receiver != null)
                    {
                        _receiver.OnMessage(eventHubClient, new ReceivedMessageEventArgs(msg));
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        public async Task<bool> ReadMessages()
        {
            if (eventHubClient == null)
                return false;

            try
            {
                var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

                var tasks = new List<Task>();
                foreach (string partition in d2cPartitions)
                {
                    tasks.Add(ReceiveMessagesFromDeviceAsync(partition, _ctsRead.Token));
                }
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {

            }

            return true;
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);

            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    EventData eventData = await eventHubReceiver.ReceiveAsync();
                    if (eventData == null)
                        return;

                    string data = Encoding.UTF8.GetString(eventData.GetBytes());
                    object msg = MessageHelper.ToObject(data);
                    if (msg != null)
                    {
                        msgReceiveQueue.Add(msg);
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
