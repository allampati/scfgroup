using Scf.Net.Base;
using Scf.Net.BlobStorage;
using ScfAzureResourceDeployment.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScfAzureResourceDeployment.Controllers
{
    public class ProjectController : Controller
    {

        // GET: Project
        public ActionResult Index()
        {
            return PartialView(GetProjects());
        }

        [HttpPost]
        public ActionResult Project(string projectId)
        {
            return PartialView(GetProject(projectId));
        }

        [HttpPost]
        public ActionResult ProjectDetails()
        {
            return PartialView(GetProjects());
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

        private Project GetProject(string projectName)
        {
            string fileName = projectName.Trim() + ".json";

            Project project = null;

            string data = BlobFile.GetFileAsString(fileName);
            project = (Project)JsonSerializer.ToObject(data);

            return project;
        }

        public void CreateResourceGroup(string projectName, string name, string region)
        {
            Project project = GetProject(projectName);
            if (project == null)
                return;

            if (project.ResourceGroups == null)
                project.ResourceGroups = new List<ResourceGroup>();

            ResourceGroup group = new ResourceGroup();
            group.Name = name;
            group.Region = region;
            project.ResourceGroups.Add(group);

            SaveProject(project);
        }

        public void SaveProject(Project project)
        {
            if (project == null)
                return;

            try
            {
                string projectInfo = JsonSerializer.ToString(project);
                string localFile = Path.Combine(Path.GetTempPath(), project.FileName);

                System.IO.File.WriteAllText(localFile, projectInfo);

                BlobFile.UploadFile(localFile);
            }
            catch (Exception ex)
            {

            }
        }
    }
}