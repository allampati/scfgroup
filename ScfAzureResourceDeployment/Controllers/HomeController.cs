using Scf.Net.Base;
using Scf.Net.BlobStorage;
using ScfAzureResourceDeployment.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScfAzureResourceDeployment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetProjects());
        }

        [HttpPost]
        public ActionResult Create(string name, string description)
        {
            try
            {
                Project project = new Project();
                project.Name = name;
                project.Description = description;
                project.CreatedDate = DateTime.Now;
                project.LastUpdatedDate = DateTime.Now;
                project.FileName = name + ".json";

                string projectInfo = JsonSerializer.ToString(project);
                string localFile = Path.Combine(Path.GetTempPath(), project.FileName);

                System.IO.File.WriteAllText(localFile, projectInfo);

                BlobFile.UploadFile(localFile);
            }
            catch(Exception ex)
            {

            }

            return RedirectToAction("Refresh");
        }

        [HttpPost]
        public ActionResult Refresh()
        {
            return View(GetProjects());
        }

        private List<Project> GetProjects()
        {
            List<string> projectitems = BlobFile.ListFiles();
            List<Project> projects = new List<Project>();
            foreach (string item in projectitems)
            {
                string data = BlobFile.GetFileAsString(item);
                Project project = (Project)JsonSerializer.ToObject(data);
                if (project != null)
                {
                    projects.Add(project);
                }
            }

            return projects;
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}