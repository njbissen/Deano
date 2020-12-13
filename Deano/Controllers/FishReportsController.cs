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
using System.Threading.Tasks;
using Deano.Models.Static;

namespace Deano.Controllers
{
	public class FishReportsController : BaseController
	{
		private string controllerName = "FishReports";

		[ActionName("Chippewa-Flowage-Hayward")]
		public ActionResult Index(string search)
		{
			Deano.Data.User user = manager.GetUser();
			if (user != null && user.RoleId == 1)
			{
				ViewBag.Role = user.Role.Name;
			}
			else
			{
				ViewBag.Role = "Guest";
			}

			JavaScriptSerializer serializer = new JavaScriptSerializer();
			//todo
			IEnumerable<string> tags = manager.GetTags(ArticleTypes.Report);
			ViewBag.Tags = new MvcHtmlString(serializer.Serialize(tags));
			ViewBag.Search = string.IsNullOrWhiteSpace(search) ? "recent" : search;
			GetMetaTags(controllerName, "Index");
			return PartialView();
		}

		public ActionResult _Topics()
		{
			return new JsonResult() { Data = manager.GetReportTopics(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult SaveBookmark(int reportId, bool mark)
		{
			manager.SaveBookmark(null, reportId);
			return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult _Index(string search, int? yearId, int? monthId, int? firstIndex, int? selectedIndex)
		{
			Catalog<prototypes.Library, prototypes.Report> catalog = manager.GetLibraryCatalog(search, yearId, monthId, firstIndex, selectedIndex);
			return new JsonResult() { Data = catalog, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult Details(int id)
		{
			if (Request.IsAuthenticated)
			{
				prototypes.Report model = null;
				if (id > 0)
				{
					// model = manager.GetReport(id);
				}
				else
				{
					model = new Report() { TemporaryId = Guid.NewGuid().ToString() };
				}
				return PartialView(model);
			}
			else
			{
				prototypes.User user = new prototypes.User();
				return PartialView("AccessDenied", user);
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Details(Data.Report model)
		{
			JsonResponse response = new JsonResponse();
			try
			{
				response.Success = true;
				manager.SaveReport(model);
				response.Messages.Add(new JsonMessage("Fish Report Saved.", JsonMessageType.Info));
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

		[ActionName("Archive-Report")]
		public ActionResult ReportArchive(int? id)
		{

			int previousId, nextId;
			IEnumerable<prototypes.Report> models = manager.GetReportArchive(id, out previousId, out nextId);
			//ViewBag.NextPost = nextId;
			//ViewBag.PreviousPost = previousId;
			return PartialView(models);
		}

		[HttpPost()]
		public string UploadReportDocuments(string temporaryId, HttpPostedFileBase fileData)
		{
			JsonResponse response = new JsonResponse();
			List<string> paths = null;
			response.Success = true;
			UploadAttachments(ref response, out paths);
			manager.SaveReportPictures(temporaryId, paths);
			return "Success";
		}

	}
}
