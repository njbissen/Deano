using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deano.Models.Business;
using System.IO;
using AcumenRegistry.Models.Prototypes.Shared;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Deano.Models.Static;
using System.Web.Script.Serialization;
using Deano.Data;
using Deano.Models.Transformations.Users;
using prototypes = Deano.Models.Prototypes.Users;
using System.Net;

namespace Deano.Controllers
{
    public class BaseController : Controller
    {
        protected Manager manager = new Manager();

        public BaseController()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public ActionResult Header()
        {
            return PartialView();
        }

        public bool UploadDocument(string filePath, HttpPostedFileBase file, string documentName, out string fileName)
        {
            string extension = string.Empty;
            string directoryPath = string.Empty;
            string documentPath = string.Empty;

            if (this.Request.Files.Count > 0)
            {
                if (file.ContentLength > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(filePath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    directoryPath = di.FullName;
                    extension = Path.GetExtension(file.FileName);

                    ImageFormat format = ImageFormat.Png;
                    switch (extension.ToUpper())
                    {
                        case ".PNG":
                            format = ImageFormat.Png;
                            break;
                        case ".GIF":
                            format = ImageFormat.Gif;
                            break;
                        case ".JPEG":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".JPG":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".BMP":
                            format = ImageFormat.Bmp;
                            break;
                    }

                    string fullName = Path.Combine(directoryPath, documentName + extension);
                    using (Image img = Image.FromStream(file.InputStream))
                    {
                        decimal percent = (decimal)400 / img.Width;

                        int width = int.Parse(Math.Ceiling(img.Width * percent).ToString());
                        int height = int.Parse(Math.Ceiling(img.Height * percent).ToString());
                        Image thumbNail = new Bitmap(width, height, img.PixelFormat);
                        Graphics g = Graphics.FromImage(thumbNail);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Rectangle rect = new Rectangle(0, 0, width, height);
                        g.DrawImage(img, rect);
                        fullName = Path.Combine(directoryPath, documentName + extension);
                        thumbNail.Save(fullName, format);
                    }
                    //file.SaveAs(fullName);
                    fileName = documentName + extension;

                    using (Image img = Image.FromFile(fullName))
                    {
                        decimal percent = (decimal)100 / img.Width;

                        int width = int.Parse(Math.Ceiling(img.Width * percent).ToString());
                        int height = int.Parse(Math.Ceiling(img.Height * percent).ToString());
                        Image thumbNail = new Bitmap(width, height, img.PixelFormat);
                        Graphics g = Graphics.FromImage(thumbNail);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Rectangle rect = new Rectangle(0, 0, width, height);
                        g.DrawImage(img, rect);
                        fullName = Path.Combine(directoryPath, "gallery" + documentName + extension);
                        thumbNail.Save(fullName, format);
                    }

                    return true;
                }
            }
            fileName = string.Empty;
            return false;
        }

        public void UploadAttachments(ref JsonResponse response, out List<string> paths)
        {
            paths = new List<string>();
            string fileName = string.Empty;
            string title = Request.Form["photo"];
            foreach (string key in Request.Files.AllKeys)
            {
                String ext = Path.GetExtension(Request.Files[key].FileName).ToUpper();
                string documentName = Guid.NewGuid().ToString();
                if (!string.IsNullOrWhiteSpace(ext))
                {
                    if (ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".GIF")
                    {
                        string root = Server.MapPath("~/Pictures");
                        DirectoryInfo di = new DirectoryInfo(root);
                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        if (UploadDocument(root, Request.Files[key], documentName, out fileName))
                        {
                            paths.Add(fileName);
                            response.Messages.Add(new JsonMessage("Photo uploaded successfully."));
                        }
                        else
                        {
                            response.Messages.Add(new JsonMessage("Photo upload failed."));
                            response.Success = false;
                        }
                    }
                    else
                    {
                        response.Messages.Add(new JsonMessage("Invalid file type. Please upload a .doc or .pdf file."));
                        response.Success = false;
                    }
                }
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable<prototypes.Tag> tags = TagTransformation.Transform(manager.GetTags(), string.Empty);
            ViewBag.RecentTags = new MvcHtmlString(serializer.Serialize(tags));
            //string name = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "." + filterContext.ActionDescriptor.ActionName;
            //ViewBag.Title = manager.GetMetaTag(MetaTagTypes.Title, name);
            //ViewBag.Keyword = manager.GetMetaTag(MetaTagTypes.Keyword, name);
            //ViewBag.Description = manager.GetMetaTag(MetaTagTypes.Description, name);
        }

        protected void GetMetaTags(string controller, string action)
        {
            string name = controller + "." + action;
            ViewBag.Title = manager.GetMetaTag(MetaTagTypes.Title, name);
            ViewBag.Keyword = manager.GetMetaTag(MetaTagTypes.Keyword, name);
            ViewBag.Description = manager.GetMetaTag(MetaTagTypes.Description, name);
        }
    }
}
