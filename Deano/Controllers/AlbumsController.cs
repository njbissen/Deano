using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcumenRegistry.Models.Prototypes.Shared;
using AcumenRegistry.Models.Shared;
using System.IO;
using Deano.Models.Prototypes.Shared;
using prototypes = Deano.Models.Prototypes.Users;
using Deano.Models.Prototypes.Users;
using System.Web.Script.Serialization;

namespace Deano.Controllers
{
    public class AlbumsController : BaseController
    {
        private string controllerName = "Albums";

        [ActionName("Chippewa-Flowage-Hayward")]
        public ActionResult Index()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable<string> tags = manager.GetAlbumTags();
            ViewBag.Tags = new MvcHtmlString(serializer.Serialize(tags));
            GetMetaTags(controllerName, "Index");
            return PartialView();
        }

        public ActionResult _Index(string album, string search, int? firstIndex, int? selectedIndex)
        {
            Catalog<prototypes.Album, prototypes.Photo> catalog = manager.GetAlbum(album, search, firstIndex, selectedIndex);
            return new JsonResult() { Data = catalog, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult _Albums()
        {
            return new JsonResult() { Data = manager.GetAlbums(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
