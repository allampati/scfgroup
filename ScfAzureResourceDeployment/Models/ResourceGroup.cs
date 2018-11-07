using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScfAzureResourceDeployment.Models
{
    public class ResourceGroup : Resource
    {
        public List<BlobStorage> BlobStorages { get; set; }
        public List<IotHub> IotHubs { get; set; }
        public List<EventHub> EventHubs { get; set; }
        public List<ServiceBus> ServiceBuses { get; set; }
        public List<NotificationHub> NotificationHubs { get; set; }


        public string ToDisplayText()
        {
            string display = "";

            display += "Name: " + Name + "<br>" + "<br>";
            display += "Region: " + Region + "<br>" + "<br>";
            display += "Description: " + Description + "<br>" + "<br>";
            display += "Tags: " + (Tags != null ? Tags.ToString() : "") + "<br>" + "<br>";
            display += "Resources" + "<br>" + "<br>";

            if (BlobStorages != null)
            {
                display += "<b>Blob Storage Accounts</b>" + "<br>" + "<br>";
                foreach (BlobStorage storage in BlobStorages)
                {
                    display += "Name: " + storage.Name + "<br>" + "<br>";
                    display += "Region: " + storage.Region + "<br>" + "<br>";
                    display += "Description: " + storage.Description + "<br>" + "<br>";
                    display += "Tags: " + (storage.Tags != null ? storage.Tags.ToString() : "") + "<br>" + "<br>";
                }
            }

            if (IotHubs != null)
            {
                display += "<b>IOT Hubs</b>" + "<br>" + "<br>";
                foreach (IotHub hub in IotHubs)
                {
                    display += "Name: " + hub.Name + "<br>" + "<br>";
                    display += "Region: " + hub.Region + "<br>" + "<br>";
                    display += "Description: " + hub.Description + "<br>" + "<br>";
                    display += "Tags: " + (hub.Tags != null ? hub.Tags.ToString() : "") + "<br>" + "<br>";
                }
            }

            if (NotificationHubs != null)
            {
                display += "<b>Notification Hubs</b>" + "<br>" + "<br>";
                foreach (NotificationHub hub in NotificationHubs)
                {
                    display += "Name: " + hub.Name + "<br>" + "<br>";
                    display += "Region: " + hub.Region + "<br>" + "<br>";
                    display += "Description: " + hub.Description + "<br>" + "<br>";
                    display += "Tags: " + (hub.Tags != null ? hub.Tags.ToString() : "") + "<br>" + "<br>";
                }
            }

            if (ServiceBuses != null)
            {
                display += "<b>Service Buses</b>" + "<br>" + "<br>";
                foreach (ServiceBus bus in ServiceBuses)
                {
                    display += "Name: " + bus.Name + "<br>" + "<br>";
                    display += "Region: " + bus.Region + "<br>" + "<br>";
                    display += "Description: " + bus.Description + "<br>" + "<br>";
                    display += "Tags: " + (bus.Tags != null ? bus.Tags.ToString() : "") + "<br>" + "<br>";
                }
            }

            if (EventHubs != null)
            {
                display += "<b>Event Hubs</b>" + "<br>" + "<br>";
                foreach (EventHub hub in EventHubs)
                {
                    display += "Name: " + hub.Name + "<br>" + "<br>";
                    display += "Region: " + hub.Region + "<br>" + "<br>";
                    display += "Description: " + hub.Description + "<br>" + "<br>";
                    display += "Tags: " + (hub.Tags != null ? hub.Tags.ToString() : "") + "<br>" + "<br>";
                }
            }

            return display;
        }
    }
}