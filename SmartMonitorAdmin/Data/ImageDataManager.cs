using SmartMonitorAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SmartMonitorAdmin.Data
{
    public static class ImageDataManager
    {
        private static List<Image> _imageList = null;

        public static List<Image> ImageList {
            get
            {
                if (_imageList != null)
                    return _imageList;

                Load();

                return _imageList;
            }
        }
        public static void Load()
        {
            if (_imageList == null)
                _imageList = new List<Image>();

            string directoryOfImage = HttpContext.Current.Server.MapPath("~/Images/");
            XDocument imageData = XDocument.Load(directoryOfImage + @"/ImageMetaData.xml");
            var images = from image in imageData.Descendants("image") select new Image(image.Element("name").Value, image.Element("filename").Value, image.Element("description").Value);

            _imageList.AddRange(images.ToList<Image>());
        }

        public static void Add(Image image, HttpPostedFileBase filebase)
        {
            string imagerep = HttpContext.Current.Server.MapPath("~/Images/");
            filebase.SaveAs(imagerep + image.Path);
            _imageList.Add(image);
            XElement xml = new XElement("images", from i in _imageList
                                                  orderby image.Path
                                                  select new XElement("image",
                                                       new XElement("name", i.Name),
                                                      new XElement("filename", i.Path),
                                                      new XElement("description", i.Description))
                                                     );
            XDocument doc = new XDocument(xml);
            doc.Save(imagerep + "/ImageMetaData.xml");
        }
    }
}