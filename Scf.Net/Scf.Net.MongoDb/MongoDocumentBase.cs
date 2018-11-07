using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.MongoDb
{
    public class MongoDocumentBase
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string StringId { get { return Id.ToString(); } }
    }
}
