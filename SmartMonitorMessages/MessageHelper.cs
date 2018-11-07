using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SmartMonitorMessages
{
    public static class MessageHelper
    {
        public static string ToString(BaseMessage msgObject)
        {
            string message = "";

            msgObject.Timestamp = DateTime.UtcNow;

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            message = JsonConvert.SerializeObject(msgObject, settings);

            return message;
        }

        public static BaseMessage ToObject(string message)
        {
            BaseMessage msgObject = null;

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            msgObject = (BaseMessage)JsonConvert.DeserializeObject(message, settings);

            return msgObject;
        }
    }
}
