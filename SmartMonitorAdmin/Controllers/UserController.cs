using Scf.Net.BlobStorage;
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
    public class UserController : Controller
    {
        // GET: User
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            //var items = await SmartMonitorDataManager<User>.GetItemsAsync(d => !d.Completed);
            List<User> items = BlobTable<User>.GetAllItems();

            return View(items);
        }
    }
}