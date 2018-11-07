using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.ServiceBusQueue
{
    public static class MessageHelper
    {
        public static string ToString(object msgObject)
        {
            string message = "";

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            message = JsonConvert.SerializeObject(msgObject, settings);

            return message;
        }

        public static object ToObject(string message)
        {
            object msgObject = null;

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            msgObject = JsonConvert.DeserializeObject(message, settings);

            return msgObject;
        }
    }
}
