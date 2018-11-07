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
    public static class Sender
    {
        private static BlockingCollection<object> msgSendQueue = new BlockingCollection<object>();
        private static CancellationTokenSource _ctsSend = new CancellationTokenSource();
        private static QueueClient _client = null;
        private static IEventFeedback _feedback = null;

        public static void Initialize(string connectionString, string queueName, IEventFeedback feedback)
        {
            _feedback = feedback;
            _client = QueueClient.CreateFromConnectionString(connectionString);

            Task.Run(async () =>
            {
                await SendQueueMessagesAsync(_ctsSend.Token);
            });
        }

        public static void Terminate()
        {
            _ctsSend.Cancel();
        }

        private async static Task SendQueueMessagesAsync(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    object msgInfo = msgSendQueue.Take();

                    var commandMessage = new BrokeredMessage(MessageHelper.ToString(msgInfo));

                    Task.Run(async () =>
                    {
                        await _client.SendAsync(commandMessage);
                    });
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
