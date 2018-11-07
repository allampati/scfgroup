using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scf.Net.Azure.ResourceManager
{
    public class ResourceDeploymentManager
    {
        private static ResourceDeploymentManager _manager = new ResourceDeploymentManager();
        public static ResourceDeploymentManager Instance { get { return _manager; } }

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

        public ResourceInfo DeployTemplate(string name, string resourceGroup, string template)
        {
            ResourceInfo resourceInfo = null;

            IDeployment result = null;
            Task.Run(async () =>
            {
                try
                {
                    result = await azure.Deployments
                    .Define(name)
                    .WithExistingResourceGroup(resourceGroup)
                    .WithTemplate(template)
                    .WithParameters("{}")
                    .WithMode(Microsoft.Azure.Management.ResourceManager.Fluent.Models.DeploymentMode.Incremental)
                    .CreateAsync();
                }
                catch (Exception ex)
                {
                    if (feedback != null)
                        feedback.OnTaskException(ex);
                }
            }).Wait();

            
            if (result != null)
            {
                resourceInfo = new ResourceInfo();
                resourceInfo.Name = result.Name;
                resourceInfo.Id = name;
            }

            return resourceInfo;
        }

        public ResourceInfo DeployIotHub(string hubName, string hubSku, string hubTier, int hubCapacity, string region, string resourceGroup)
        {
            ResourceInfo resourceInfo = null;

            string template = Properties.Templates.IotHub;
            template = template.Replace("{hubName}", hubName);
            template = template.Replace("{location}", region);
            template = template.Replace("{sku}", hubSku);
            template = template.Replace("{skuName}", hubTier);
            template = template.Replace("{skuCapacity}", hubCapacity.ToString());

            IDeployment result = null;
            Task.Run(async () =>
            {
                try
                {
                    result = await azure.Deployments
                    .Define(hubName)
                    .WithExistingResourceGroup(resourceGroup)
                    .WithTemplate(template)
                    .WithParameters("{}")
                    .WithMode(Microsoft.Azure.Management.ResourceManager.Fluent.Models.DeploymentMode.Incremental)
                    .CreateAsync();
                }
                catch (Exception ex)
                {
                    if (feedback != null)
                        feedback.OnTaskException(ex);
                }
            }).Wait();

            if(result != null)
            {
                resourceInfo = new ResourceInfo();
                resourceInfo.Name = result.Name;
                resourceInfo.Id = hubName;
            }

            return resourceInfo;
        }


        public ResourceInfo DeployNotificationHub(string hubName, string hubNamespace, string hubSku, string hubTier, string region, string resourceGroup)
        {
            ResourceInfo resourceInfo = null;

            string template = Properties.Templates.NotificationHub;
            template = template.Replace("{hubName}", hubName);
            template = template.Replace("{location}", region);
            template = template.Replace("{hubNamespace}", hubNamespace);
            template = template.Replace("{hubSku}", hubSku);
            template = template.Replace("{hubTier}", hubTier);

            IDeployment result = null;
            Task.Run(async () =>
            {
                try
                {
                    result = await azure.Deployments
                    .Define(hubName)
                    .WithExistingResourceGroup(resourceGroup)
                    .WithTemplate(template)
                    .WithParameters("{}")
                    .WithMode(Microsoft.Azure.Management.ResourceManager.Fluent.Models.DeploymentMode.Incremental)
                    .CreateAsync();
                }
                catch (Exception ex)
                {
                    if (feedback != null)
                        feedback.OnTaskException(ex);
                }
            }).Wait();

            if (result != null)
            {
                resourceInfo = new ResourceInfo();
                resourceInfo.Name = result.Name;
                resourceInfo.Id = hubName;
            }

            return resourceInfo;
        }

        public void GetAllDeployments()
        {
            Task.Run(async () =>
            {
                try
                {
                    IPagedCollection<IDeployment> list = await azure.Deployments.ListAsync();
                    if(list != null)
                    {
                        int n = 0;
                    }
                }
                catch (Exception ex)
                {
                    if (feedback != null)
                        feedback.OnTaskException(ex);
                }
            }).Wait();
        }
    }
}
