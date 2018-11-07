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
    public class GeneralSettingsController : Controller
    {
        //// GET: Settings
        //public ActionResult Index()
        //{
        //    Task.Run(async () =>
        //    { 
        //        var items = await SmartMonitorDataManager<Setting>.GetItemsAsync(d => !d.Completed);
        //        return PartialView(items);
        //    }).Wait();

        //    return PartialView();
        //}

        //public ActionResult Edit(string id)
        //{
        //    Task.Run(async () =>
        //    {
        //        var item = await SmartMonitorDataManager<Setting>.GetItemAsync(id);
        //        return PartialView(item);
        //    }).Wait();

        //    return PartialView();
        //}

        // GET: Parameter
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            //var items = await SmartMonitorDataManager<Setting>.GetItemsAsync(d => !d.Completed);
            List<Setting> items = Scf.Net.BlobStorage.BlobTable<Setting>.GetAllItems();
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
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Units,Serial,Type,LogToDatabase,MonitoringInterval, MonitoringIntervalType, Description,Completed")] Setting item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<Setting>.CreateItemAsync(item);
                Scf.Net.BlobStorage.BlobTable<Setting>.Upsert(item.Id, item);
                return RedirectToAction("Index");
            }

            return PartialView(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Units,Serial,Type,LogToDatabase,MonitoringInterval, MonitoringIntervalType,Description,Completed")] Setting item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<Setting>.UpdateItemAsync(item.Id, item);
                Scf.Net.BlobStorage.BlobTable<Setting>.Upsert(item.Id, item);
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

            //Setting item = await SmartMonitorDataManager<Setting>.GetItemAsync(id);
            Setting item = Scf.Net.BlobStorage.BlobTable<Setting>.GetItem(id);
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

            //Setting item = await SmartMonitorDataManager<Setting>.GetItemAsync(id);
            Setting item = BlobTable<Setting>.GetItem(id);
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
            BlobTable<Setting>.Delete(id);

            return RedirectToAction("Index");
        }

    }
}