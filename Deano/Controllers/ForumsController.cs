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
using Deano.Models.Static;

namespace Deano.Controllers
{
	public class ForumsController : BaseController
	{
		private string controllerName = "Forums";

		[ActionName("Chippewa-Flowage-Hayward")]
		public ActionResult Index(string search)
		{
			var user = manager.GetUser();
			if (user != null && user.Role == "Administrator")
			{
				ViewBag.Role = "Admin";
			}
			else if (user != null)
			{
				ViewBag.Role = user.Role;
			}
			else
			{
				ViewBag.Role = "Guest";
			}

			JavaScriptSerializer serializer = new JavaScriptSerializer();
			IEnumerable<string> tags = manager.GetTags(ArticleTypes.Post);
			ViewBag.Search = string.IsNullOrWhiteSpace(search) ? "recent" : search;
			ViewBag.Tags = new MvcHtmlString(serializer.Serialize(tags));
			GetMetaTags(controllerName, "Index");
			return PartialView();
		}

		public ActionResult _Index()
		{
			return new JsonResult() { Data = manager.GetForumTopics(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult Threads(int forumId, int? firstIndex, int? selectedIndex)
		{
			Catalog<prototypes.Forum, prototypes.Thread> catalog = manager.GetForumCatalog(forumId, firstIndex, selectedIndex);
			return new JsonResult() { Data = catalog, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult Posts(int threadId, string search, int? firstIndex, int? selectedIndex)
		{
			Catalog<prototypes.Thread, prototypes.Post> catalog = manager.GetPostCatalog(threadId, search, firstIndex, selectedIndex);
			return new JsonResult() { Data = catalog, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult ForumDetails(int? threadId)
		{
			return PartialView();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ForumDetails(Data.Forum model)
		{
			JsonResponse response = new JsonResponse();
			try
			{
				response.Success = true;
				List<string> paths = null;
				UploadAttachments(ref response, out paths);
				manager.SaveForumTopic(model);
				response.Messages.Add(new JsonMessage("Forum Saved.", JsonMessageType.Info));
			}
			catch (Exception ex)
			{
				//ErrorServices.HandleError(ex);
				response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
			}

			return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response };
			//return new AjaxJsonResult(new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response });
		}

		public ActionResult _Threads(int? firstIndex, int? selectedIndex)
		{
			Pager pager = new Pager(firstIndex.GetValueOrDefault(), 18, selectedIndex.GetValueOrDefault());
			return new JsonResult() { Data = pager, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		[ActionName("Archive-Post")]
		public ActionResult PostArchive(int? id)
		{
			int previousId, nextId;
			IEnumerable<prototypes.Post> models = manager.GetPostArchive(id, out previousId, out nextId);
			ViewBag.NextPost = nextId;
			ViewBag.PreviousPost = previousId;
			return PartialView(models);
		}

		public ActionResult PostDetails(int threadId, int? postId)
		{
			if (Request.IsAuthenticated)
			{
				prototypes.Post model = null;
				if (postId.GetValueOrDefault() > 0)
				{
					model = manager.GetPost(postId.Value);
				}
				else
				{
					model = new Post() { ThreadId = threadId, TemporaryId = Guid.NewGuid().ToString() };
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
		public ActionResult PostDetails(Data.Post model)
		{
			JsonResponse response = new JsonResponse();
			try
			{
				response.Success = true;
				//List<string> paths = null;
				//UploadAttachments(ref response, out paths);
				manager.SavePost(model);
				response.Messages.Add(new JsonMessage("Post Saved.", JsonMessageType.Info));
			}
			catch (Exception ex)
			{
				//ErrorServices.HandleError(ex);
				response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
			}
			//make sure response.Id is set even if save fails
			//response.Id = model.RegistryEmployeeId;
			//return new AjaxJsonResult(new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response });
			return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response };
		}

		[HttpPost()]
		public string UploadPostDocuments(string temporaryId, HttpPostedFileBase fileData)
		{
			JsonResponse response = new JsonResponse();
			List<string> paths = null;
			response.Success = true;
			UploadAttachments(ref response, out paths);
			manager.SavePostPictures(temporaryId, paths);
			return "Success";
		}

		public ActionResult SaveBookmark(int postId, bool mark)
		{
			manager.SaveBookmark(postId, null);
			return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public ActionResult ThreadDetails(int forumId, int? threadId)
		{
			if (Request.IsAuthenticated)
			{
				prototypes.Thread model = new Thread() { ForumId = forumId, TemporaryId = Guid.NewGuid().ToString() };
				return PartialView(model);
			}
			else
			{
				prototypes.User user = new prototypes.User();
				return PartialView("AccessDenied", user);
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ThreadDetails(prototypes.Thread model)
		{
			JsonResponse response = new JsonResponse();
			try
			{
				response.Success = true;
				manager.SaveThread(model);
				response.Messages.Add(new JsonMessage("Thread Saved.", JsonMessageType.Info));
			}
			catch (Exception ex)
			{
				//ErrorServices.HandleError(ex);
				response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
			}
			return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = response };
		}
	}
}
