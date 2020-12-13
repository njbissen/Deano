using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deano.Data;
using prototypes = Deano.Models.Prototypes.Users;
using System.Web.Security;
using AcumenRegistry.Models.Prototypes.Shared;

namespace Deano.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            prototypes.User user = new prototypes.User() { UserId = -1 };
            return PartialView(user);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotPassword(prototypes.User model)
        {
            JsonResponse response = null;
            //if (this.ModelState.IsValid)
            //{
            if (manager.SendForgotPasswordEmail(model.Username, out response))
            {

            }
            //}
            //else
            //{
            //    response.Messages.Add(new JsonMessage("ModelState is invalid.", JsonMessageType.Error));
            //}

            return new JsonResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult CreateAccount()
        {
            return PartialView(manager.CreateRegistration());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAccount(User model)
        {
            JsonResponse response = null;

            if (this.ModelState.IsValid)
            {
                if (manager.SaveRegistration(model, out response))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                }
            }
            else
            {
                response.Messages.Add(new JsonMessage("ModelState is invalid.", JsonMessageType.Error));
            }

            return new JsonResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult LogIn(string username, string password)
        {
            JsonResponse response = new JsonResponse();
            if (manager.ValidateCredentials(username, password, out response))
            {
                Deano.Data.User user = manager.GetUser(username);
                FormsAuthentication.SetAuthCookie(username, false);
                response.Role = user.Role.Name;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return new JsonResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            JsonResponse response = new JsonResponse();
            response.Success = true;
            return new JsonResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
