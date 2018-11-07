using Scf.Net.Base;
using Scf.Net.BlobStorage;
using Scf.Net.PushHub;
using SmartMonitorAdmin.Data;
using SmartMonitorAdmin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SmartMonitorAdmin
{
    public class MvcApplication : System.Web.HttpApplication, IEventFeedback
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string connString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
            BlobTable<Parameter>.Initialize(connString);
            BlobTable<Account>.Initialize(connString);
            BlobTable<SmartDevice>.Initialize(connString);
            BlobTable<Setting>.Initialize(connString);
            BlobTable<UserGroup>.Initialize(connString);
            BlobTable<User>.Initialize(connString);
            BlobTable<Alarm>.Initialize(connString);

            BlobContent.Initialize(connString, this);

            string pushConnString = ConfigurationManager.AppSettings["PushHubConnectionString"];
            string hubname = ConfigurationManager.AppSettings["PushHubName"];
            PushManager.Start(hubname, pushConnString);

            //SmartMonitorDataManager<Parameter>.Initialize("Parameters");
            //SmartMonitorDataManager<Account>.Initialize("Account");
            //SmartMonitorDataManager<SmartDevice>.Initialize("SmartDevices");
            //SmartMonitorDataManager<Setting>.Initialize("Settings");
            //SmartMonitorDataManager<UserGroup>.Initialize("UserGroups");
            //SmartMonitorDataManager<User>.Initialize("Users");

            //PushNotificationManager.Instance.Initialize();
            //AzureActiveDirectoryManager.Instance.Initialize();
            //IoTHubEventManager.Instance.Initialize();
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
