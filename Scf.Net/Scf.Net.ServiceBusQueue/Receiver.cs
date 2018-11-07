using Microsoft.ServiceBus.Messaging;
using Scf.Net.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scf.Net.ServiceBusQueue
{
    public class Receiver
    {
        private BlockingCollection<object> msgReceiveQueue = new BlockingCollection<object>();
        private CancellationTokenSource _ctsReceive = new CancellationTokenSource();
        private QueueClient _client = null;
        private IEventFeedback _feedback = null;
        private IEventReceiver _receiver = null;

        private string _connectionString = "";
        private string _queueName = "";

        public Receiver(string connectionString, string queueName, IEventFeedback feedback)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        public void Initialize(IEventReceiver receiver)
        {
            _receiver = receiver;

            _client = QueueClient.CreateFromConnectionString(_connectionString, _queueName);

            Task.Run(async () =>
            {
                await ReceiveQueueMessagesAsync(_ctsReceive.Token);
            });

            _client.OnMessage(message =>
            {
                object msgInfo = MessageHelper.ToObject(message.GetBody<string>());
                msgReceiveQueue.Add(msgInfo);
            });
        }

        public void Terminate()
        {
            _ctsReceive.Cancel();
        }

        private async Task ReceiveQueueMessagesAsync(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    object msgInfo = msgReceiveQueue.Take();
                    if (_receiver != null)
                        _receiver.OnMessage(_client, new ReceivedMessageEventArgs(msgInfo));
                }
                catch (Exception ex)
                {
                    if (_feedback != null)
                        _feedback.OnException(_client, ex);
                }
            }
        }
    }
}
