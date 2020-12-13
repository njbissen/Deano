using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Deano
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             "FishReports", // Route name
             "Fish-Reports/{action}/{id}", // URL with parameters
             new { controller = "FishReports", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
           "Photos", // Route name
           "Fish-Photos/{action}/{id}", // URL with parameters
           new { controller = "Albums", action = "Index", id = UrlParameter.Optional } // Parameter defaults
          );
            routes.MapRoute(
           "Forums", // Route name
           "Fish-Forums/{action}/{id}", // URL with parameters
           new { controller = "Forums", action = "Index", id = UrlParameter.Optional } // Parameter defaults
          );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //ModelBinders.Binders.Add(typeof(AcumenRegistry.Data.Address), new AcumenRegistry.Models.ModelBinders.NullablePropertyModelBinder());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}