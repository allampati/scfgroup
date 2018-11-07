using Scf.Net.BlobStorage;
using SmartMonitorAdmin.Data;
using SmartMonitorAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartMonitorAdmin.Controllers
{
    public class ParameterController : Controller
    {
        // GET: Parameter
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            //var items = await SmartMonitorDataManager<Parameter>.GetItemsAsync(d => !d.Completed);
            List<Parameter> items = BlobTable<Parameter>.GetAllItems();
            return View(items);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return PartialView();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Units,Serial,Type,LogToDatabase,MonitoringInterval, MonitoringIntervalType, Description,Completed")] Parameter item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<Parameter>.CreateItemAsync(item);
                BlobTable<Parameter>.Upsert(item.Id, item);
                return RedirectToAction("Index");
            }

            return PartialView(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Units,Serial,Type,LogToDatabase,MonitoringInterval, MonitoringIntervalType,Description,Completed")] Parameter item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<Parameter>.UpdateItemAsync(item.Id, item);
                BlobTable<Parameter>.Upsert(item.Id, item);

                return RedirectToAction("Index");
            }

            return PartialView(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Parameter item = await SmartMonitorDataManager<Parameter>.GetItemAsync(id);
            Parameter item = BlobTable<Parameter>.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Parameter item = await SmartMonitorDataManager<Parameter>.GetItemAsync(id);
            Parameter item = BlobTable<Parameter>.GetItem(id);

            BlobTable<Parameter>.Delete(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            //await SmartMonitorDataManager<Parameter>.DeleteItemAsync(id);
            Scf.Net.BlobStorage.BlobTable<Parameter>.Delete(id);
            return RedirectToAction("Index");
        }

        [ActionName("Alarms")]
        public async Task<ActionResult> AlarmsAsync(string id)
        {
            Parameter item = null;

            //item = await SmartMonitorDataManager<Parameter>.GetItemAsync(id);
            item = BlobTable<Parameter>.GetItem(id);
            item.Alarms = BlobTable<Alarm>.GetAllItems();
            item.Alarms = item.Alarms.FindAll(x => x.DeviceId == id);
            ViewData["parameterId"] = item.Id;

            return (item != null) ? PartialView(item.Alarms) : PartialView();
        }

        [HttpPost]
        public ActionResult SaveAlarms(string id, Alarm[] alarms)
        {
            Parameter item = null;
            ViewData["parameterId"] = id;

            //Task.Run(async () =>
            //{
                //item = await SmartMonitorDataManager<Parameter>.GetItemAsync(id);
                item = BlobTable<Parameter>.GetItem(id);

            if (item != null)
            {
                if (item.Alarms != null)
                    item.Alarms.Clear();
                else
                    item.Alarms = new List<Alarm>();

                foreach (Alarm alarm in alarms)
                {
                    alarm.DeviceId = item.Id;
                    item.Alarms.Add(alarm);
                    BlobTable<Alarm>.Upsert(alarm.Id, alarm);
                }
            }
            //}).Wait();

            return RedirectToAction("Index");
        }
    }
}