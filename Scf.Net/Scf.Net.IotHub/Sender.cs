using Microsoft.Azure.Devices;
using Scf.Net.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scf.Net.IotHub
{
    public class MessageInfo
    {
        public object Message { get; set; }
        public string[] DeviceIds { get; set; }
    }

    public class Sender
    {
        static ServiceClient serviceClient;

        static BlockingCollection<MessageInfo> msgSendQueue = new BlockingCollection<MessageInfo>();
        static BlockingCollection<SendStatusEventArgs> msgFeedbackQueue = new BlockingCollection<SendStatusEventArgs>();

        private static CancellationTokenSource _ctsSend = new CancellationTokenSource();
        private static CancellationTokenSource _ctsNotify = new CancellationTokenSource();


        private static IEventFeedback _feedback = null;
        private static IEventSender _sender = null;

        private string _connectionString = "";

        public Sender(string connectionString, IEventFeedback feedback)
        {
            _connectionString = connectionString;
            _feedback = feedback;
        }

        public bool Initialize(IEventSender senderFeedback)
        {
            _sender = senderFeedback;

            serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);

            Task.Run(async () =>
            {
                await SendCloudToDeviceMessageAsync(_ctsSend.Token);
            });

            Task.Run(async () =>
            {
                await ReceiveFeedbackAsync(_ctsSend.Token);
            });

            Task.Run(async () =>
            {
                await NotifyEvents();
            });

            return true;
        }

        public void Stop()
        {
            _ctsSend.Cancel();
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
                    object msg = msgFeedbackQueue.Take();
                    if (msg != null && _sender != null)
                    {
                        _sender.OnMessage(serviceClient, new ReceivedMessageEventArgs(msg));
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        private async static Task SendCloudToDeviceMessageAsync(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    MessageInfo msgInfo = msgSendQueue.Take();
                    var commandMessage = new Message(Encoding.ASCII.GetBytes(MessageHelper.ToString(msgInfo.Message)));
                    commandMessage.Ack = DeliveryAcknowledgement.Full;
                    foreach (string deviceId in msgInfo.DeviceIds)
                    {
                        await serviceClient.SendAsync(deviceId, commandMessage);
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        private async static Task ReceiveFeedbackAsync(CancellationToken ct)
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                    if (feedbackBatch == null)
                        continue;

                    foreach (FeedbackRecord record in feedbackBatch.Records)
                    {
                        SendStatusEventArgs status = new SendStatusEventArgs
                        {
                            OriginalMessageId = record.OriginalMessageId,
                            Description = record.Description,
                            DeviceId = record.DeviceId,
                            Status = (SendStatusEventArgs.StatusCode)record.StatusCode

                        };

                        msgFeedbackQueue.Add(status);
                    }

                    await feedbackReceiver.CompleteAsync(feedbackBatch);
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
