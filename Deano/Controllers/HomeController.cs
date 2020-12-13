using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcumenRegistry.Models.Prototypes.Shared;
using AcumenRegistry.Models.Shared;
using System.IO;

namespace Deano.Controllers
{
    public class HomeController : BaseController
    {
        private string controllerName = "Home";
       
        [ActionName("fishing-guide-home")]
        public ActionResult Index()
        {
            GetMetaTags(controllerName, "Index");
            return View();
        }

        [ActionName("Index")]
        public ActionResult Home()
        {
            GetMetaTags(controllerName, "Index");
            return View("fishing-guide-home");
        }

        public ActionResult IndexTan()
        {
            return View();
        }

        public ActionResult IndexBlue()
        {
            return View();
        }

        public ActionResult FishReports()
        {
            return PartialView();
        }

        public ActionResult Slideshow()
        {
            return PartialView();
        }

        public ActionResult Deano()
        {
            return PartialView();
        }

        public ActionResult SiteMap()
        {
            FileStream strm = new FileStream(Server.MapPath("/Content/SiteMap.txt"), FileMode.Open);
            return new FileStreamResult(strm, "text/plain");
        }

        [ActionName("fishing-guide-request")]
        public ActionResult About()
        {
            GetMetaTags(controllerName, "About");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MessageDetails(Deano.Data.Message model, string[] catchType)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                if (catchType != null)
                {
                    model.Catch = string.Join(", ", catchType);
                }
                manager.SendMessage(model, out response);
                response.Success = true;
                response.Messages.Add(new JsonMessage("Message Sent.", JsonMessageType.Info));
            }
            catch (Exception ex)
            {
                //ErrorServices.HandleError(ex);
                response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
            }
            //make sure response.Id is set even if save fails
            //response.Id = model.RegistryEmployeeId;
            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response };
        }

        [ActionName("fishing-guide-rates")]
        public ActionResult Rates()
        {
            GetMetaTags(controllerName, "Rates");
            return View();
        }

        public ActionResult Sponsors()
        {
            return PartialView();
        }

        public ActionResult Testimonials()
        {
            return PartialView();
        }

        public ActionResult ContactUs()
        {
            return PartialView();
        }

        public ActionResult LocalEvents()
        {
            return PartialView();
        }

        [ActionName("fishing-guide-directions")]
        public ActionResult Directions()
        {
            GetMetaTags(controllerName, "Directions");
            return View();
        }
    }
}
