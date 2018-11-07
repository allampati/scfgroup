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
    public class UserGroupController : Controller
    {
        // GET: UserGroup
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            //var items = await SmartMonitorDataManager<UserGroup>.GetItemsAsync(d => !d.Completed);
            List<UserGroup> items = await Task.Run(()=>BlobTable<UserGroup>.GetAllItems());

            return View(items);
        }
    }
}