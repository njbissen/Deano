using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcumenRegistry.Models.Shared
{
    public class AjaxJsonResult : JsonResult
    {
        public JsonResult Result { get; set; } 

        public AjaxJsonResult(JsonResult result)
        {
            this.Result = result;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("<textarea>");
            this.Result.ExecuteResult(context); 
            context.HttpContext.Response.Write("</textarea>"); 
            context.HttpContext.Response.ContentType = "text/html"; 
        }
    }
}