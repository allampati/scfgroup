using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using Microsoft.Rest.Azure.Authentication;
using Scf.Net.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Azure.ResourceManager
{
    public class ResourceGroupManager
    {
        private static ResourceGroupManager _manager = new ResourceGroupManager();
        public static ResourceGroupManager Instance { get { return _manager; } }

        private IAzure azure = null;
        private IEventFeedback feedback = null;

        public bool Initialize(object iAzure, IEventFeedback iFeedback)
        {
            azure = iAzure as IAzure;
            feedback = iFeedback;

            return true;
        }

        public void Terminate()
        {

        }

        public bool Create(string groupName, string region, IDictionary<string, string> tags = null)
        {
            try
            {
                Region regionType = Region.Create(region);
                if (regionType == null)
                    regionType = Region.USCentral;

                bool bExists = false;
                Task.Run(async () =>
                {
                    bExists = await azure.ResourceGroups.CheckExistenceAsync(groupName);
                    
                }).Wait();

                if (bExists)
                    return true;

                Task.Run(async () =>
                {
                    await azure.ResourceGroups
                            .Define(groupName)
                            .WithRegion(regionType)
                            .CreateAsync();
                }).Wait();
            }
            catch(Exception ex)
            {
                if (feedback != null)
                    feedback.OnException(this, ex);

                return false;
            }

            return true;
        }


        public bool Delete(string groupName)
        {
            try
            {
                Task.Run(async () =>
                {
                    await azure.ResourceGroups.DeleteByNameAsync(groupName);
                });
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnException(this, ex);

                return false;
            }

            return true;
        }

        public ResourceInfo GetInfo(string groupName)
        {
            ResourceInfo info = new ResourceInfo();

            try
            {
                Task.Run(async () =>
                {
                    IResourceGroup group = await azure.ResourceGroups.GetByNameAsync(groupName);
                    if (group != null)
                    {
                        info.Name = group.Name;
                        info.Type = group.Type;
                        info.Id = group.Id;
                        info.Key = group.Key;
                        info.ProvisioningState = group.ProvisioningState;
                        info.RegionName = group.RegionName;
                        info.Tags = (group.Tags != null) ? group.Tags.ToList() : null;
                    }                   
                }).Wait();
            }
            catch(Exception ex)
            {
                if (feedback != null)
                    feedback.OnException(this, ex);

                return null;
            }

            return info;
        }

        public bool AddTag(string groupName, string tagKey, string tagValue)
        {
            try
            {
                Task.Run(async () =>
                {
                    var group = await azure.ResourceGroups.GetByNameAsync(groupName);
                    var update = group
                                .Update()
                                .WithTag(tagKey, tagValue)
                                .Apply();
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnException(this, ex);
                return false;
            }

            return true;
        }

        public bool AddTags(string groupName, IDictionary<string, string> tags)
        {
            try
            {
                Task.Run(async () =>
                {
                    var group = await azure.ResourceGroups.GetByNameAsync(groupName);
                    var update = group.Update();
                    foreach (KeyValuePair<string, string> item in tags)
                    {
                        update.WithTag(item.Key, item.Value);
                    }
                    update.Apply();
                }).Wait();
            }
            catch(Exception ex)
            {
                if (feedback != null)
                    feedback.OnException(this, ex);

                return false;
            }

            return true;
        }

        public bool ReplaceTags(string groupName, IDictionary<string, string> tags)
        {
            try
            {
                Task.Run(async () =>
                {
                    var group = await azure.ResourceGroups.GetByNameAsync(groupName);
                    var update = group
                        .Update()
                        .WithTags(tags)
                        .Apply();
                }).Wait();
            }
            catch (Exception ex)
            {
                if (feedback != null)
                    feedback.OnException(this, ex);

                return false;
            }

            return true;
        }
    }
}
