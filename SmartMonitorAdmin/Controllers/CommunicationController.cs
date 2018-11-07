using CheckBoxList.Mvc;
using Newtonsoft.Json;
using Scf.Net.BlobStorage;
using Scf.Net.PushHub;
using SmartMonitorAdmin.Data;
using SmartMonitorAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartMonitorAdmin.Controllers
{
    public class SelectedItem
    {
        public string Id { get; set; }
        public bool Selected { get; set; }
    }
    public class CommunicationController : Controller
    {
        // GET: Communication
        public ActionResult Index()
        {
            Communication comm = new Communication();

            List<SmartDevice> list = BlobTable<SmartDevice>.GetAllItems();
            if(list != null)
            {
                comm.SelectedDevices = list;
            }
            //Task.Run(async () =>
            //{
            //    var items = await SmartMonitorDataManager<SmartDevice>.GetItemsAsync(d => !d.Completed);
            //    comm.SelectedDevices = items.ToList();
            //}).Wait();

            return View(comm);
        }

        [HttpPost]
        public ActionResult SaveSelected(string data)
        {
            SelectedItem[] checkedItems = JsonConvert.DeserializeObject<SelectedItem[]>(data);
            List<SmartDevice> items = BlobTable<SmartDevice>.GetAllItems();
            if (items != null && items.Count > 0)
            {
                foreach (SelectedItem chkitem in checkedItems)
                {
                    int index = items.FindIndex(i => i.Id == chkitem.Id);
                    SmartDevice item = (index >= 0) ? items[index] : null;
                    if (item != null)
                    {
                        item.Selected = chkitem.Selected;
                        BlobTable<SmartDevice>.Update(item);
                    }
                }
            }
            //Task.Run(async () =>
            //{
            //    var tmpitem = await SmartMonitorDataManager<SmartDevice>.GetItemsAsync(d => !d.Completed);
            //    List<SmartDevice> items = tmpitem.ToList();
            //    foreach (SelectedItem chkitem in checkedItems)
            //    {
            //        int index = items.FindIndex(i => i.Id == chkitem.Id);
            //        SmartDevice item = (index >= 0) ? items[index] : null;
            //        if (item != null)
            //        {
            //            item.Selected = chkitem.Selected;
            //            await SmartMonitorDataManager<SmartDevice>.UpsertItemAsync(item);
            //        }
            //    }
            //});

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendMessage(string message, string data)
        {
            string[] checkedItems = JsonConvert.DeserializeObject<string[]>(data);
            List<SmartDevice> items = BlobTable<SmartDevice>.GetAllItems();
            if (items != null && items.Count > 0)
            {
                foreach (string chkitem in checkedItems)
                {
                    int index = items.FindIndex(i => i.Id == chkitem);
                    SmartDevice item = (index >= 0) ? items[index] : null;
                    if (item != null)
                    {
                        Scf.Net.PushHub.PushMessage msg = new Scf.Net.PushHub.PushMessage();
                        msg.DeviceId = item.DeviceId;
                        msg.Message = message;
                        msg.Type = Scf.Net.PushHub.PushMessage.PushType.Unknown;

                        switch (item.Platform)
                        {
                            case SmartDevice.PlatformType.Android:
                                msg.Type = Scf.Net.PushHub.PushMessage.PushType.Android;
                                break;
                            case SmartDevice.PlatformType.Apple:
                                msg.Type = Scf.Net.PushHub.PushMessage.PushType.Apple;
                                break;
                            case SmartDevice.PlatformType.UWP:
                                msg.Type = Scf.Net.PushHub.PushMessage.PushType.Windows;
                                break;
                            case SmartDevice.PlatformType.Windows:
                                msg.Type = Scf.Net.PushHub.PushMessage.PushType.WindowsPhone;
                                break;
                        }
                        PushManager.SendMessage(msg);
                    }                    
                }
            }
            //Task.Run(async () =>
            //{
            //    var tmpitem = await SmartMonitorDataManager<SmartDevice>.GetItemsAsync(d => !d.Completed);
            //    List<SmartDevice> items = tmpitem.ToList();
            //    foreach (string itemid in checkedItems)
            //    {
            //        int index = items.FindIndex(i => i.Id == itemid);
            //        SmartDevice item = (index >= 0) ? items[index] : null;
            //        if (item != null)
            //        {
            //            PushMessage msg = new PushMessage();
            //            msg.DeviceId = item.DeviceId;
            //            msg.Message = message;
            //            msg.Type = PushMessage.PushType.Unknown;

            //            switch (item.Platform)
            //            {
            //                case SmartDevice.PlatformType.Android:
            //                    msg.Type = PushMessage.PushType.Android;                                
            //                    break;
            //                case SmartDevice.PlatformType.Apple:
            //                    msg.Type = PushMessage.PushType.Apple;
            //                    break;
            //                case SmartDevice.PlatformType.UWP:
            //                    msg.Type = PushMessage.PushType.Windows;                                
            //                    break;
            //                case SmartDevice.PlatformType.Windows:
            //                    msg.Type = PushMessage.PushType.Windows;
            //                    break;
            //            }
            //            PushNotificationManager.Instance.SendMessage(msg);
            //        }
            //    }
            //});
            return RedirectToAction("Index");
        }
    }
}