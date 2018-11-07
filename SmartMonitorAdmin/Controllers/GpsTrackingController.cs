using Scf.Net.Base;
using Scf.Net.BlobStorage;
using SmartMonitorAdmin.Data;
using SmartMonitorAdmin.Models;
using SmartMonitorMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMonitorAdmin.Controllers
{
    public class GpsTrackingController : Controller, IEventFeedback, IEventReceiver
    {
        private GpsMap _mapData = new GpsMap();
        private static BlobQueue _gpsQueue = null;

        public GpsTrackingController()
        {
            if (_gpsQueue == null)
            {
                _gpsQueue = new BlobQueue("gpsmessages", ConfigurationManager.AppSettings["BlobStorageConnectionString"], this, this);
                _gpsQueue.Initialize();
            }
        }
        
        // GET: GpsTracking
        public ActionResult Index()
        {
            return View(_mapData);
        }

        [HttpPost]
        public JsonResult Refresh()
        {

            return Json(_mapData, JsonRequestBehavior.AllowGet);
        }


        public bool OnMessage(object sender, ReceivedMessageEventArgs args)
        {
            GpsMessage msg = args.Message as GpsMessage;
            if (msg != null)
            {
                _mapData.UpdateLocation(msg.DeviceId, msg.Latitude, msg.Longitude);
            }

            //RedirectToAction("Index", "GpsTracking");
            return true;
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