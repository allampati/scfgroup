using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Scf.Net.BlobStorage;
using SmartMonitorAdmin.Data;
using SmartMonitorAdmin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmartMonitorAdmin.Controllers
{
    public class SmartDeviceController : Controller
    {
        // GET: SmartDevice
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            //var items = await SmartMonitorDataManager<SmartDevice>.GetItemsAsync(d => !d.Completed);
            List<SmartDevice> items = BlobTable<SmartDevice>.GetAllItems();
            return View(items);
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,UserId,DeviceId,DeviceKey,Description,SerialNumber,Platform, Manufacturer, Model,GpsTracking,Latitude,Longitude,Completed")] SmartDevice item)
        {
            if (ModelState.IsValid)
            {
                await CreateIoTDevice(item);
                BlobTable<SmartDevice>.Upsert(item.Id, item);
                //await SmartMonitorDataManager<SmartDevice>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "RowKey,Name,UserId,DeviceId,DeviceKey,Description,SerialNumber,Platform, Manufacturer, Model,GpsTracking,Latitude,Longitude,Completed")] SmartDevice item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<SmartDevice>.UpdateItemAsync(item.Id, item);
                BlobTable<SmartDevice>.Upsert(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //SmartDevice item = await SmartMonitorDataManager<SmartDevice>.GetItemAsync(id);
            SmartDevice item = BlobTable<SmartDevice>.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //SmartDevice item = await SmartMonitorDataManager<SmartDevice>.GetItemAsync(id);
            SmartDevice item = BlobTable<SmartDevice>.GetItem(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            //await SmartMonitorDataManager<SmartDevice>.DeleteItemAsync(id);
            BlobTable<SmartDevice>.Delete(id);

            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> Details(string id)
        {
            //var item = await SmartMonitorDataManager<SmartDevice>.GetItemAsync(id);
            SmartDevice item = BlobTable<SmartDevice>.GetItem(id);

            return PartialView(item);
        }

        private async Task<string> CreateIoTDevice(SmartDevice item)
        {
            string deviceKey = "";
            var registryManager = RegistryManager.CreateFromConnectionString(ConfigurationManager.AppSettings["IoTHubConnectionString"]);
            try
            {
                item.DeviceId = Guid.NewGuid().ToString();
                var device = await registryManager.AddDeviceAsync(new Device(item.DeviceId));

                item.DeviceKey = device.Authentication.SymmetricKey.PrimaryKey;
            }
            catch (DeviceAlreadyExistsException)
            {
            }

            return deviceKey;
        }
    }
}