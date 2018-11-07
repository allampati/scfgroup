using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Storage.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Azure.ResourceManager
{
    public class BlobStorageManager
    {
        private static BlobStorageManager _manager = new BlobStorageManager();
        public static BlobStorageManager Instance { get { return _manager; } }

        private IAzure azure = null;
        private IFeedback feedback = null;

        public bool Initialize(object iAzure, IFeedback iFeedback)
        {
            azure = iAzure as IAzure;
            feedback = iFeedback;

            return true;
        }

        public void Terminate()
        {

        }

        public ResourceInfo Create(string storageName, string region, string resourceGroup, IDictionary<string, string> tags = null)
        {
            ResourceInfo resourceInfo = null;
            try
            {                
                Region regionType = Region.Create(region);
                if (regionType == null)
                    regionType = Region.USCentral;

                CheckNameAvailabilityResult result = null;
                Task.Run(async () =>
                {
                    result = await azure.StorageAccounts.CheckNameAvailabilityAsync(storageName);
                    
                }).Wait();

                if(result != null && result.IsAvailalbe != null)
                {
                    bool available = (bool)result.IsAvailalbe;
                    if (!available)
                        return resourceInfo;
                }

                Task.Run(async () =>
                {
                    IStorageAccount storageAcct = null;
                    if (tags != null)
                    {
                        storageAcct = await azure.StorageAccounts
                        .Define(storageName)
                        .WithRegion(region)
                        .WithExistingResourceGroup(resourceGroup)
                        .WithBlobStorageAccountKind()
                        .WithTags(tags)
                        .CreateAsync();
                    }
                    else
                    {
                        storageAcct = await azure.StorageAccounts
                        .Define(storageName)
                        .WithRegion(region)
                        .WithExistingResourceGroup(resourceGroup)
                        .WithBlobStorageAccountKind()
                        .CreateAsync();
                    }
                   
                    resourceInfo = GetInfo(storageAcct);

                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);

                return resourceInfo;
            }

            return resourceInfo;
        }

        public bool Delete(string storageName)
        {
            string storageId = GetIdFromName(storageName);
            if (storageId == "")
                return false;

            return DeleteFromId(storageId);
        }

        private bool DeleteFromId(string storageId)
        {
            try
            {
                Task.Run(async () =>
                {
                    await azure.StorageAccounts.DeleteByIdAsync(storageId);
                });
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);

                return false;
            }

            return true;
        }

        private string GetIdFromName(string storageName)
        {
            string id = "";

            try
            {
                Task.Run(async () =>
                {
                    IPagedCollection<IStorageAccount> list = await azure.StorageAccounts.ListAsync();
                    foreach(IStorageAccount acct in list)
                    {
                        if(acct.Name == storageName)
                        {
                            id = acct.Id;
                            break;
                        }
                    }
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);

                return id;
            }

            return id;
        }

        private ResourceInfo GetInfo(IStorageAccount item)
        {
            ResourceInfo info = null;
            if (item != null)
            {
                info = new ResourceInfo();
                info.Name = item.Name;
                info.Type = item.Type;
                info.Id = item.Id;
                info.Key = item.Key;
                info.ProvisioningState = item.ProvisioningState.ToString();
                info.RegionName = item.RegionName;
                info.Tags = (item.Tags != null) ? item.Tags.ToList() : null;
            }

            return info;
        }

        public ResourceInfo GetInfo(string storageName)
        {
            string storageId = GetIdFromName(storageName);
            if (storageId == "")
                return null;

            return GetInfoFromId(storageId);
        }

        private ResourceInfo GetInfoFromId(string storageId)
        {
            ResourceInfo info = null;

            try
            {
                Task.Run(async () =>
                {
                    IStorageAccount item = await azure.StorageAccounts.GetByIdAsync(storageId);
                    info = GetInfo(item);
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);

                return null;
            }

            return info;
        }

        public bool AddTag(string storageId, string tagKey, string tagValue)
        {
            try
            {
                Task.Run(async () =>
                {
                    var group = await azure.StorageAccounts.GetByIdAsync(storageId);
                    var update = group
                                .Update()
                                .WithTag(tagKey, tagValue)
                                .Apply();
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);
                return false;
            }

            return true;
        }

        public bool AddTags(string storageId, IDictionary<string, string> tags)
        {
            try
            {
                Task.Run(async () =>
                {
                    var group = await azure.StorageAccounts.GetByIdAsync(storageId);
                    var update = group.Update();
                    foreach (KeyValuePair<string, string> item in tags)
                    {
                        update.WithTag(item.Key, item.Value);
                    }
                    update.Apply();
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);

                return false;
            }

            return true;
        }

        public bool ReplaceTags(string storageId, IDictionary<string, string> tags)
        {
            try
            {
                Task.Run(async () =>
                {
                    var group = await azure.StorageAccounts.GetByIdAsync(storageId);
                    var update = group
                        .Update()
                        .WithTags(tags)
                        .Apply();
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnTaskException(ex);

                return false;
            }

            return true;
        }
    }
}
