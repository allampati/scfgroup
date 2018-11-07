using Scf.Net.BlobStorage;
using SmartMonitorAdmin.Data;
using SmartMonitorAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SmartMonitorAdmin.Controllers
{
    public class ImageController : Controller
    {
        private string containerName = "images";
        // GET: Image
        public ActionResult Index()
        {            
            return View(BlobContent.ListImages(containerName));
        }

        public ActionResult Add()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save()
        {            
            foreach (string name in Request.Files)
            {
                var file = Request.Files[name];

                string fileName = System.IO.Path.GetFileName(file.FileName);

                BlobContent.UploadImage(containerName, fileName);

                //string imagename = file.FileName;
                //int index = imagename.IndexOf('.');
                //imagename = (index > 0) ? imagename.Substring(0, index) : imagename;
                //imagename = imagename.Replace(' ', '_');

                //Image image = new Image(imagename, fileName, Request["description"]);

                //ImageDataManager.Add(image, file);
            }
            return HttpNotFound();
        }

    }
}