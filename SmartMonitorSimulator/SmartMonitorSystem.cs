using Microsoft.Azure.Devices.Client;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartMonitorSimulator
{
    public class GpsSimulationInfo
    {
        public class Point
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Point(double lat, double lon)
            {
                Latitude = lat;
                Longitude = lon;
            }
        }

        public enum SimulationType { City, Country, World };

        public int Interval { get; set; }
        public int Type { get; set; }
        public bool Running { get; set; }

        public List<Point> City { get; set; }
        public List<Point> Country { get; set; }
        public List<Point> World { get; set; }

        public GpsSimulationInfo()
        {
            // City List
            List<Point> city = new List<Point>();

            City = city;

            // Country List
            List<Point> country = new List<Point>();

            country.Add(new Point(32.3617, -86.2792));
            country.Add(new Point(58.3014, - 134.422));
            country.Add(new Point(33.45, - 112.067));
            country.Add(new Point(34.7361, - 92.3311));
            country.Add(new Point(38.5556, - 121.469));
            country.Add(new Point(39.7618, - 104.881));
            country.Add(new Point(41.7627, - 72.6743));
            country.Add(new Point(39.1619, - 75.5267));
            country.Add(new Point(30.455, - 84.2533));
            country.Add(new Point(33.755, - 84.39));
            country.Add(new Point(21.3, - 157.817));
            country.Add(new Point(43.6167, - 116.2));
            country.Add(new Point(39.6983, - 89.6197));
            country.Add(new Point(39.791, - 86.148));
            country.Add(new Point(41.5908, - 93.6208));
            country.Add(new Point(39.0558, - 95.6894));
            country.Add(new Point(38.197, - 84.863));
            country.Add(new Point(30.45, - 91.14));
            country.Add(new Point(44.307, - 69.782));
            country.Add(new Point(38.9729, - 76.5012));
            country.Add(new Point(42.3581, - 71.0636));
            country.Add(new Point(42.7336, - 84.5467));
            country.Add(new Point(44.9442, - 93.0936));
            country.Add(new Point(32.2989, - 90.1847));
            country.Add(new Point(38.5767, - 92.1736));
            country.Add(new Point(46.5958, - 112.027));
            country.Add(new Point(40.8106, - 96.6803));
            country.Add(new Point(39.1608, - 119.754));
            country.Add(new Point(43.2067, - 71.5381));
            country.Add(new Point(40.2237, - 74.764));
            country.Add(new Point(35.6672, - 105.964));
            country.Add(new Point(42.6525, - 73.7572));
            country.Add(new Point(35.7667, - 78.6333));
            country.Add(new Point(46.8133, - 100.779));
            country.Add(new Point(39.9833, - 82.9833));
            country.Add(new Point(35.4822, - 97.535));
            country.Add(new Point(44.9308, - 123.029));
            country.Add(new Point(40.2697, - 76.8756));
            country.Add(new Point(41.8236, - 71.4222));
            country.Add(new Point(34.0006, - 81.0347));
            country.Add(new Point(44.368, - 100.336));
            country.Add(new Point(36.1667, - 86.7833));
            country.Add(new Point(30.25, - 97.75));
            country.Add(new Point(40.75, - 111.883));
            country.Add(new Point(44.2597, - 72.575));
            country.Add(new Point(37.5333, - 77.4667));
            country.Add(new Point(47.0425, - 122.893));
            country.Add(new Point(38.3472, - 81.6333));
            country.Add(new Point(43.0667, - 89.4));
            country.Add(new Point(41.1456, - 104.802));

            Country = country;

            // World List
            List<Point> world = new List<Point>();

            World = world;
        }
    }


    public class SmartMonitorSystem
    {
        private static SmartMonitorSystem instance = new SmartMonitorSystem();

        public static SmartMonitorSystem Instance
        {
            get { return instance; }
        }

        static DeviceClient deviceClient;
        static Queue<BaseMessage> msgReceiveQueue = new Queue<BaseMessage>();

        Thread msgSendThread;
        Thread msgReceiveThread;
        Queue<BaseMessage> msgSendQueue = new Queue<BaseMessage>();
        string iotHubUri = "scfhub.azure-devices.net";
        string deviceKey = "i3aKam/CTDokTt96PGpUxSGglRPt5xhMa+mhkvX18A8=";
        string deviceId = "e37f4872-d332-46f8-a74f-27b37383e953";
        int msgcounter = 1;
        private static Object thisLock = new Object();

        public void Initialize()
        {
            DeviceAuthenticationWithRegistrySymmetricKey key = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);
            deviceClient = DeviceClient.Create(iotHubUri, key, TransportType.Mqtt);

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            msgSendThread = new Thread(new ThreadStart(SendMessagesToCloud));

            // Start the thread
            msgSendThread.Start();

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            msgReceiveThread = new Thread(new ThreadStart(ReceiveMessagesFromCloud));

            // Start the thread
            msgReceiveThread.Start();
        }

        public void Terminate()
        {
            if(msgSendThread != null)
            {
                msgSendThread.Abort();
            }
        }

        public GpsSimulationInfo GpsInfo { get; set; }

        private string AsBase64String(string value)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        public List<BaseMessage> GetReceivedMessages()
        {
            if (msgReceiveQueue.Count <= 0)
                return null;

            List<BaseMessage> list = new List<BaseMessage>();

            lock (thisLock)
            {
                while (msgReceiveQueue.Count > 0)
                {
                    BaseMessage msg = msgReceiveQueue.Dequeue();
                    list.Add(msg);
                }
            }

            return list;
        }

        public void StartGpsSimulation(GpsSimulationInfo info)
        {
            if (info == null)
                return;

            GpsInfo = info;
            GpsInfo.Running = true;

            Task.Run(async () =>
            {
                int counter = 0;
                while (GpsInfo.Running)
                {
                    GpsMessage msg = new GpsMessage();
                    msg.Id = msgcounter.ToString();
                    msg.DeviceId = deviceId;
                    if (counter < GpsInfo.Country.Count)
                    {
                        msg.Latitude = GpsInfo.Country[counter].Latitude;
                        msg.Longitude = GpsInfo.Country[counter].Longitude;
                        msgSendQueue.Enqueue(msg);
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }

                    await Task.Delay(GpsInfo.Interval);

                    msgcounter++;
                }                
            });
        }

        public void StopGpsSimulation()
        {
            GpsInfo.Running = false;
        }
       
        private void SendMessagesToCloud()
        {
            while(true)
            {
                while (msgSendQueue.Count > 0)
                {
                    BaseMessage msg = msgSendQueue.Dequeue();
                    if (msg == null)
                        continue;

                    string messageString = MessageHelper.ToString(msg);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    Task.Run(async () =>
                    {
                        await deviceClient.SendEventAsync(message);
                    });
                }
                Thread.Sleep(1000);
            }
        }

        private void ReceiveMessagesFromCloud()
        {
            while(true)
            {
                Task.Run(async () =>
                {
                    await ReceiveC2dAsync();
                });

                Thread.Sleep(1000);
            }
        }

        private static async Task<bool> ReceiveC2dAsync()
        {
            Message receivedMessage = await deviceClient.ReceiveAsync();
            if (receivedMessage == null)
                return false;

            lock (thisLock)
            {
                BaseMessage msg = MessageHelper.ToObject(Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                if (msg != null)
                    msgReceiveQueue.Enqueue(msg);
            }

            await deviceClient.CompleteAsync(receivedMessage);

            return true;
        }
    }
}
