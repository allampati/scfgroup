using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.BlobStorage
{
    public static class BlobTable<T> where T : BaseTable
    {
        private static CloudStorageAccount storageAccount = null;
        private static CloudTableClient tableClient = null;
        private static CloudTable table = null;
        private static string partitionKey = "Default";

        public static void Initialize(string connectionString)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            table = GetTable(typeof(T).Name);
        }

        public static void Terminate()
        {

        }

        public static string[] GetDataTypes()
        {
            return Enum.GetNames(typeof(EdmType));
        }

        public static bool CreateTable(string name)
        {
            try
            {
                CloudTable table = tableClient.GetTableReference(name);

                table.CreateIfNotExists();
            }
            catch(Exception ex)
            {

            }

            return true;
        }

        public static bool DeleteTable(string name)
        {
            try
            {
                // Create the CloudTable that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(name);

                // Delete the table it if exists.
                table.DeleteIfExists();
            }
            catch(Exception ex)
            {

            }

            return true;
        }

        private static CloudTable GetTable(string name)
        {
            try
            {
                CreateTable(name);

                return tableClient.GetTableReference(name);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static List<T> GetAllItems()
        {
            return GetItems(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
        }

        public static List<T> GetItems(string filter)
        {
            if (table == null)
                return null;
            List<T> list = new List<T>();

            try
            {

                TableQuery query = new TableQuery().Where(filter);
                List<DynamicTableEntity> records = table.ExecuteQuery(query).ToList();

                Type entityModelType = typeof(T);
                foreach (DynamicTableEntity record in records)
                {
                    T newObject = (T)Activator.CreateInstance(entityModelType);
                    foreach (var prop in record.Properties)
                    {
                        PropertyInfo entityProp = entityModelType.GetProperty(prop.Key);
                        if (entityProp != null)
                        {
                            if (entityProp.PropertyType.IsEnum)
                                entityProp.SetValue(newObject, Enum.Parse(entityProp.PropertyType, prop.Value.StringValue, true), null);
                            else
                                entityProp.SetValue(newObject, prop.Value.PropertyAsObject);
                        }
                    }

                    BaseTable tbitem = newObject as BaseTable;
                    tbitem.Id = record.RowKey;
                    tbitem.Partition = record.PartitionKey;
                    tbitem.Tag = record.ETag;

                    list.Add(newObject);
                }
            }
            catch(Exception ex)
            {

            }

            return list;
        }

        public static T GetItem(string rowId)
        {
            if (table == null)
                return null;

            try
            {
                List<T> list = GetItems(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowId));
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public static bool Insert(T item)
        {
            if (table == null)
                return false;

            try
            {
                PropertyInfo[] properties = item.GetType().GetProperties();

                ElasticTableEntity record = new ElasticTableEntity();
                BaseTable tbitem = item as BaseTable;
                record.RowKey = (tbitem.Id == null) ? Guid.NewGuid().ToString() : tbitem.Id;
                record.PartitionKey = tbitem.Partition;
                record.ETag = tbitem.Tag;

                foreach (PropertyInfo info in properties)
                {
                    var value = info.GetValue(item, null);
                    record.Properties.Add(info.Name, EntityProperty.CreateEntityPropertyFromObject(value));
                }


                TableOperation insertOrReplaceOperation = TableOperation.Insert(record);

                table.Execute(insertOrReplaceOperation);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Upsert(string rowId, T item)
        {
            if (table == null)
                return false;

            try
            {
                if(rowId == null)
                {
                    Insert(item);
                    return true;
                }

                TableOperation retrieveOperation = TableOperation.Retrieve(partitionKey, rowId);
                TableResult result = table.Execute(retrieveOperation);
                if (result != null && result.Result != null)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Update(T item)
        {
            if (table == null)
                return false;

            try
            {
                PropertyInfo[] properties = item.GetType().GetProperties();

                ElasticTableEntity record = new ElasticTableEntity();

                BaseTable tbitem = item as BaseTable;
                record.RowKey = tbitem.Id;
                record.PartitionKey = tbitem.Partition;
                record.ETag = tbitem.Tag;

                foreach (PropertyInfo info in properties)
                {
                    record.Properties.Add(info.Name, EntityProperty.CreateEntityPropertyFromObject(info.GetValue(item, null)));
                }

                TableOperation replaceOperation = TableOperation.Replace(record);

                table.Execute(replaceOperation);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool Delete(string rowId)
        {
            if (table == null)
                return false;

            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve(partitionKey, rowId);
                TableResult result = table.Execute(retrieveOperation);

                if (result != null || result.Result != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(result.Result as ITableEntity);

                    table.Execute(deleteOperation);
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
