using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcumenRegistry.Models.Prototypes.Shared;
using Deano.Controllers;
using prototypes = Deano.Models.Prototypes.Users;
using System.Web.Security;

namespace Deano.Areas.Accounts.Controllers
{
    public class MemberController : BaseController
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Home()
        {
            JsonResponse response = new JsonResponse();
            manager.GetMemberMessages(out response);
            ViewBag.Messages = new MvcHtmlString(response.ToString());
            return PartialView();
        }

        public ActionResult Account()
        {
            prototypes.Account model = manager.GetAccount(HttpContext.User.Identity.Name);
            return PartialView(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Account(Deano.Data.User model)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                manager.SaveAccount(model);
                response.Success = false;
                response.Messages.Add(new JsonMessage("Account Information has been updated.", JsonMessageType.Info));
            }
            catch (Exception ex)
            {
                //ErrorServices.HandleError(ex);
                response.Success = false;
                response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
            }

            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response };
        }

        public ActionResult Credentials()
        {
            prototypes.Credentials model = manager.GetCredentials(HttpContext.User.Identity.Name);
            return PartialView(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Credentials(Deano.Data.User model)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                manager.SaveCredentials(model);
                response.Success = false;
                FormsAuthentication.SetAuthCookie(model.Username, false);
                response.Messages.Add(new JsonMessage("Username and Password has been updated.", JsonMessageType.Info));
            }
            catch (Exception ex)
            {
                //ErrorServices.HandleError(ex);
                response.Success = false;
                response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
            }

            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response };
        }
    }
}
