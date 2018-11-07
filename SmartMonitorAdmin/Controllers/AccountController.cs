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
    public class AccountController : Controller
    {
        // GET: Account
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            Account acct = null;

            List<Account> items = await Task.Run(() => BlobTable<Account>.GetAllItems());
            if(items != null && items.Count > 0)
            {
                acct = items[0];
            }
            //var items = await SmartMonitorDataManager<Account>.GetItemsAsync(d => !d.Completed);
            //if (items != null)
            //{
            //    List<Account> list = items.ToList();

            //    if (list.Count > 0)
            //        acct = list[0];
            //}

            if (acct == null)
            {
                acct = new Account();
            }

            return View(acct);
        }

        [HttpPost]
        [ActionName("Save")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAsync([Bind(Include = "Id,CompanyName,Address,City,State,ZipCode,Phone,Email")] Account item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<Account>.UpsertItemAsync(item);
                await Task.Run(() => BlobTable<Account>.Upsert(item.Id, item));
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,CompanyName,Address,City,State,ZipCode,Phone,Email")] Account item)
        {
            if (ModelState.IsValid)
            {
                //await SmartMonitorDataManager<Account>.UpsertItemAsync(item);
                await Task.Run(() => BlobTable<Account>.Upsert(item.Id, item));

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

            //Account item = await SmartMonitorDataManager<Account>.GetItemAsync(id);
            Account item = BlobTable<Account>.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            //Account item = await SmartMonitorDataManager<Account>.GetItemAsync(id);
            Account item = BlobTable<Account>.GetItem(id);
            if (item == null)
                item = new Account();

            return View(item);
        }
    }
}