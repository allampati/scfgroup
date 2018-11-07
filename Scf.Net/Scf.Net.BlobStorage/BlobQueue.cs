using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Scf.Net.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scf.Net.BlobStorage
{
    public class BlobQueue
    {
        private CloudStorageAccount storageAccount = null;
        private CloudQueueClient queueClient = null;
        private CloudQueue cloudQueue = null;

        private BlockingCollection<string> msgSendQueue = new BlockingCollection<string>();
        private CancellationTokenSource _ctsReceive = new CancellationTokenSource();

        private string _connectionString = "";
        private string _queueName = "";

        private IEventFeedback _feedback = null;
        private IEventReceiver _receiver = null;

        public CloudQueue Queue { get { return cloudQueue; } }
        public CloudQueueClient QueueClient { get { return queueClient; } }
        public CloudStorageAccount QueueStorageAccount { get { return storageAccount; } }

        public BlobQueue(string queueName, string connectionString, IEventFeedback feedback, IEventReceiver receiver)
        {
            _queueName = queueName;
            _connectionString = connectionString;
            _feedback = feedback;
            _receiver = receiver;
        }

        public void Initialize()
        {
            storageAccount = CloudStorageAccount.Parse(_connectionString);
            queueClient = storageAccount.CreateCloudQueueClient();
            cloudQueue = GetQueue();

            Task.Run(async () =>
            {
                await ReceiveMessagesFromQueue(_ctsReceive.Token);
            });
        }

        public void Terminate()
        {
            _ctsReceive.Cancel();
        }

        public bool Delete()
        {
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            queue.DeleteIfExists();

            return true;
        }

        private bool Create()
        {
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            queue.CreateIfNotExists();

            return true;
        }

        private CloudQueue GetQueue()
        {
            Create();

            return queueClient.GetQueueReference(_queueName);
        }

        public int GetMessageCount()
        {
            int count = 0;

            cloudQueue.FetchAttributes();

            count = (cloudQueue.ApproximateMessageCount != null ? (int)cloudQueue.ApproximateMessageCount : 0);

            return count;
        }

        public void SendMessage(string message)
        {
            CloudQueueMessage queueMsg = new CloudQueueMessage(message);
            cloudQueue.AddMessage(queueMsg);
        }

        private async Task ReceiveMessagesFromQueue(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    CloudQueueMessage queueMsg = cloudQueue.GetMessage();
                    if(queueMsg != null && _receiver != null)
                    {
                        object msg = MessageHelper.ToObject(queueMsg.AsString);
                        bool result = _receiver.OnMessage(cloudQueue, new ReceivedMessageEventArgs(msg));
                        if(result)
                        {
                            cloudQueue.DeleteMessage(queueMsg);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
