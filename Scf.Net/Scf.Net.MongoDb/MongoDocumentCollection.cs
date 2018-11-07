using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.MongoDb
{
    public class MongoDocumentCollection<T> where T : class
    {
        private string CollectionId = "";
        private DbConnectionInfo connectionInfo = null;
        private MongoClient client;
        private IMongoDatabase db;

        public void Initialize(DbConnectionInfo connInfo)
        {
            connectionInfo = connInfo;
            CollectionId = typeof(T).Name.ToLower();

            client = new MongoClient(connectionInfo.EndpointId);
            CreateDatabaseIfNotExists();
            CreateCollectionIfNotExists();
            //BsonClassMap.RegisterClassMap<T>();
        }

        private void CreateDatabaseIfNotExists()
        {
            try
            {
                db = client.GetDatabase(connectionInfo.DatabaseId);
            }
            catch (Exception e)
            {

            }
        }

        private void CreateCollectionIfNotExists()
        {
            try
            {
                IMongoCollection<T> collection = db.GetCollection<T>(CollectionId);
                if(collection == null)
                {
                    db.CreateCollection(CollectionId);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task<List<T>> GetAllItemsAsync()
        {
            IMongoCollection<T> coll = db.GetCollection<T>(CollectionId);

            return await coll.Find(_ => true).ToListAsync();
        }

        public async Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IMongoCollection<T> coll = db.GetCollection<T>(CollectionId);

            return await coll.Find(predicate).ToListAsync();
        }

        public async Task<T> CreateItemAsync(T item)
        {
            IMongoCollection<T> coll = db.GetCollection<T>(CollectionId);

            await coll.InsertOneAsync(item);

            return item;
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            IMongoCollection<T> coll = db.GetCollection<T>(CollectionId);

            PropertyInfo prop = typeof(T).GetProperty("Id");
            object value = (prop != null) ? prop.GetValue(item) : null;
            if (value != null)
            {
                var filter = Builders<T>.Filter.Eq("Id", value);
                await coll.FindOneAndReplaceAsync(filter, item);
            }

            return item;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            IMongoCollection<T> coll = db.GetCollection<T>(CollectionId);

            var filter = Builders<T>.Filter.Eq("Id", ObjectId.Parse(id));
            await coll.DeleteOneAsync(filter);

            return true;
        }
    }
}
