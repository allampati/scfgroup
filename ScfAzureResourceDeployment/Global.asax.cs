using Scf.Net.Base;
using Scf.Net.BlobStorage;
using ScfAzureResourceDeployment.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ScfAzureResourceDeployment
{
    public class MvcApplication : System.Web.HttpApplication, IEventFeedback
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            BlobFile.Initialize(ConfigurationManager.AppSettings["BlobFileShareProjects"], ConfigurationManager.AppSettings["BlobStorageConnectionString"], this);
        }

        public void OnComplete(object sender, TaskEventArgs args)
        {

        }

        public void OnError(object sender, InternalErrorEventArgs args)
        {

        }

        public void OnException(object sender, Exception ex)
        {

        }
    }
}
