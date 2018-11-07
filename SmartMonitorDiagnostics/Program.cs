using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartMonitorDiagnostics
{
    class Program
    {
        static string connectionString = "Endpoint=sb://ihsuproddmres001dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=7BiKc00cfvCYd3MQHFyM3Wu/mRoD5PRc/IIirSPdFjU=";
        static string monitoringEndpointName = "iothub-ehub-smartmonit-211713-8b9e6f7cfa";
        static EventHubClient eventHubClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Monitoring. Press Enter key to exit.\n");

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, monitoringEndpointName);
            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;
            CancellationTokenSource cts = new CancellationTokenSource();
            var tasks = new List<Task>();

            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            }

            Console.ReadLine();
            Console.WriteLine("Exiting...");
            cts.Cancel();
            Task.WaitAll(tasks.ToArray());
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    await eventHubReceiver.CloseAsync();
                    break;
                }

                EventData eventData = await eventHubReceiver.ReceiveAsync(new TimeSpan(0, 0, 10));

                if (eventData != null)
                {
                    string data = Encoding.UTF8.GetString(eventData.GetBytes());
                    Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
                }
            }
        }
    }
}
