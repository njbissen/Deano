using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using data = Deano.Data;
using prototypes = Deano.Models.Prototypes.Users;
using AcumenRegistry.Models.Prototypes.Shared;
using Deano.Data;
using Deano.Models.Transformations.Users;
using System.Web;
using Deano.Models.Prototypes.Users;
using Deano.Models.Prototypes.Shared;
using Google.GData.Photos;
using System.IO;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Deano.Models.Static;
using Google.GData.Client;
using Google.Apis.Auth.OAuth2;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Services;
using Google.Apis.PhotosLibrary.v1;
using System.Net.Http;
using Google.Apis.PhotosLibrary.v1.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Deano.Models.Business
{
	public class Manager
	{
		private Data.Repository data = new Data.Repository();

		private string picasaUserName = "admin@deanosguideservice.com";

		private NetworkCredential GetNetworkCredentials()
		{
			string username = data.GetSetting(Settings.EmailCredentialsUsername).Value;
			string password = data.GetSetting(Settings.EmailCredentialsPassword).Value;
			NetworkCredential credentials = new NetworkCredential(username, password);
			return credentials;
		}

		private SmtpClient GetSmtpClient()
		{
			SmtpClient client = new SmtpClient();
			client.Credentials = GetNetworkCredentials();
			client.Port = 587;
			client.Host = "smtp.gmail.com";
			client.EnableSsl = true;

			return client;
		}

		public void GetMemberMessages(out JsonResponse response)
		{
			IEnumerable<Azure.Models.Post> posts = null;
			IQueryable<Deano.Azure.Models.Report> reports = null;
			response = new JsonResponse();
			var user = GetUser();

			data.GetThreadPosts(-1, "My", user.Name, out posts);
			response.Messages.Add(new JsonMessage(string.Format("You have created {0} posts.", posts.ToList().Count.ToString()), JsonMessageType.Info));

			data.GetThreadPosts(-1, "Favorites", user.Name, out posts);
			response.Messages.Add(new JsonMessage(string.Format("You have {0} favorite posts.", posts.ToList().Count.ToString()), JsonMessageType.Info));

			reports = data.GetReports("Favorites", user.Name);
			response.Messages.Add(new JsonMessage(string.Format("You have {0} favorite reports.", reports.Count().ToString()), JsonMessageType.Info));
		}

		public prototypes.User CreateRegistration()
		{
			Azure.Models.User model = new Azure.Models.User();
			return UserTransformation.Transform<prototypes.User>(model);
		}

		public bool SendMessage(Message message, out JsonResponse response)
		{
			response = new JsonResponse();
			SmtpClient client = GetSmtpClient();

			Setting displayName = data.GetSetting(Settings.RequestDisplayName);
			Setting fromAddress = data.GetSetting(Settings.NoReplyEmailAddress);
			Setting toAddress = data.GetSetting(Settings.RequestEmailAddress);
			try
			{
				message.MessageDate = DateTime.Now;
				data.SaveMessage(message);

				MailAddress maFrom = new MailAddress(fromAddress.Value, displayName.Value, Encoding.UTF8);
				MailMessage mmsg = new MailMessage();
				mmsg.From = maFrom;

				foreach (string mailTo in toAddress.Value.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
				{
					MailAddress maTo = new MailAddress(mailTo, displayName.Value, Encoding.UTF8);
					mmsg.To.Add(maTo);
				}

				string messageDate = string.Format("Message date {0}. ", message.MessageDate);
				string fromEmail = string.Format("Message email {0}. ", message.FromEmail);
				string fromName = string.Format("Message name {0}. ", message.FromName);
				string fromPhone = string.Format("Message phone {0}. ", message.FromPhone);
				string requestDate = string.Format("Request date {0}. ", message.RequestDate);
				string requestType = string.Format("Request type {0}. ", message.NumberOfDays);
				string requestSize = string.Format("Request size {0}. ", message.NumberOfPeople);
				string requestSubject = string.Format("Message subject {0}. ", message.Subject);
				string requestBody = string.Format("Message body {0}. ", message.Body);
				string requestCatch = string.Format("Request catch {0}. ", message.Catch);
				string[] segments = new string[] { messageDate, fromEmail, fromName, fromPhone, requestDate,
			   requestType,requestSize ,requestSubject,requestBody,requestCatch};

				string body = "<div>" + string.Join("</div><div>", segments) + "</div>";
				mmsg.Body = "<html><body>" + body + "</body></html>";
				mmsg.BodyEncoding = Encoding.UTF8;
				mmsg.IsBodyHtml = true;
				mmsg.Subject = "Guide Trip Request";
				mmsg.SubjectEncoding = Encoding.UTF8;

				client.Send(mmsg);
				response.Messages.Add(new JsonMessage("Your request for a trip has been sent to Deano.", JsonMessageType.Info));
				response.Success = true;
				return true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(new JsonMessage("Your password could not be sent to your email account.", JsonMessageType.Error));
				response.Success = false;
			}

			return false;
		}

		public bool SendForgotPasswordEmail(string username, out JsonResponse response)
		{
			response = new JsonResponse();
			SmtpClient client = GetSmtpClient();

			try
			{
				var account = data.GetUser(username);
				if (account == null)
				{
					response.Messages.Add(new JsonMessage("Your email address could not be found.", JsonMessageType.Warning));
					response.Success = false;
				}

				Setting domainName = data.GetSetting(Settings.DomainName);
				Setting displayName = data.GetSetting(Settings.RequestDisplayName);
				Setting fromAddress = data.GetSetting(Settings.NoReplyEmailAddress);

				MailAddress
					maFrom = new MailAddress(fromAddress.Value, displayName.Value, Encoding.UTF8),
					maTo = new MailAddress(account.Name, account.Name, Encoding.UTF8);

				MailMessage mmsg = new MailMessage(maFrom, maTo);
				string message = string.Format("Your password for {0} is {1}.", domainName.Value, Crypto.Decrypt(account.Password));
				mmsg.Body = "<html><body><span></span>" + message + "</body></html>";
				mmsg.BodyEncoding = Encoding.UTF8;
				mmsg.IsBodyHtml = true;
				mmsg.Subject = "Account Information from Deano";
				mmsg.SubjectEncoding = Encoding.UTF8;

				client.Send(mmsg);
				response.Messages.Add(new JsonMessage("Your password has been sent to your email account.", JsonMessageType.Info));
				response.Success = true;
				return true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(new JsonMessage("Your password could not be sent to your email account.", JsonMessageType.Error));
				response.Success = false;
			}

			return false;
		}

		public bool SaveRegistration(data.User model, out JsonResponse response)
		{
			response = new JsonResponse();
			try
			{
				if (!ValidateEmailAddress(-1, model.Username))
				{
					response.Messages.Add(new JsonMessage("Email Address is already in use.", JsonMessageType.Warning));
					response.Success = false;
					return false;
				}

				if (!ValidateHandle(-1, model.Handle))
				{
					response.Messages.Add(new JsonMessage("Screen Name is already in use.", JsonMessageType.Warning));
					response.Success = false;
					return false;
				}

				model.CreatedDate = DateTime.Now;
				model.Password = Crypto.Encrypt(model.Password);
				//model.Phone = model.Phone.Replace("-", "").Replace(")", "").Replace("(", "");
				model.Active = true;
				model.RoleId = 2;

				data.SaveRegistration(model);
				response.Id = model.UserId;
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Messages.Add(new JsonMessage(ex.Message, JsonMessageType.Error));
				response.Success = false;
			}
			return response.Success;
		}

		public string GetMetaTag(int typeId, string name)
		{
			string value = data.GetMetaTag(typeId, name);
			if (string.IsNullOrWhiteSpace(value))
			{
				switch (typeId)
				{
					case MetaTagTypes.Title:
						value = "Deano's Guide Service - Dean Elmer - Fish Reports from the Chippewa Flowage near Hayward, Wisconsin 54843.";
						break;
					case MetaTagTypes.Description:
						value = "Dean Elmer offers a professionally guided trip on the Chippewa Flowage near Hayward, Wisconsin. Dean is a licensed guide and will teach you how to locate good walleye spots and show you techniques that will help you to learn how to fish Walleyes like a pro. " +
							"Dean will provide all of the tackle, gear and bait and will clean your catch for you at the end of the day " +
							"Fishing guide trips are offered for a full eight hour day or a four hour half day. For groups up to four people, Dean will take you out on his twenty four foot Northwoods Pontoon boat. " +
							"Night guided fishing trips are also available.  Contact Dean Elmer at admin@deanosguideservice.com for more details or Request a Fishing Gudie Trip on deanosguideservice.com.";
						break;
					case MetaTagTypes.Keyword:
						value = "Deano's Guide Service,Fishing,Fishing Guide,Guide Service,Fishing Trips,Guide Trips,Fishing Photos,Fish Photos,Fishing Advice,Fishing Pictures,Fishing Pics,Fish Forums,Fishing Forums,Guide,Service,Hayward Wi,Hayward Wisconsin,Hayward,Wisconsin,Chippewa Flowage,Walleye,Crappie,Bass,Muskie,Muskellunge,Northern Pike,Hayward Lakes";
						break;
				}
			}
			return value;
		}

		public void SaveBookmark(int? postId, int? reportId)
		{
			if (HttpContext.Current.Request.IsAuthenticated)
			{
				prototypes.Account account = GetAccount(HttpContext.Current.User.Identity.Name);
				data.SaveBookmark(account.UserId, postId, reportId);
			}
		}

		public prototypes.Credentials GetCredentials(string name)
		{
			var model = data.GetUser(name);
			return UserTransformation.Transform<prototypes.Credentials>(model);
		}

		public Azure.Models.User GetUser()
		{
			return data.GetUser(HttpContext.Current.User.Identity.Name);
		}

		public Azure.Models.User GetUser(string name)
		{
			return data.GetUser(name);
		}

		public bool ValidateEmailAddress(int userId, string emailAddress)
		{
			var model = data.GetUser(emailAddress);
			if (model == null)
			{
				return true;
			}
			else if (model.Id == userId.ToString())
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool ValidateHandle(int userId, string handle)
		{
			Deano.Data.User model = data.GetUserByHandle(handle);
			if (model == null)
			{
				return true;
			}
			else if (model.UserId == userId)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool ValidateCredentials(string name, string password, out JsonResponse response)
		{
			response = new JsonResponse();
			var model = data.GetUser(name, Crypto.Encrypt(password));
			if (model == null)
			{
				response.Messages.Add(new JsonMessage("Invalid Username and/or Password.", JsonMessageType.Warning));
				response.Success = false;
			}
			else
			{
				response.Success = true;
			}
			return response.Success;
		}

		public prototypes.Account GetAccount(string name)
		{
			var model = data.GetUser(name);
			return UserTransformation.Transform<prototypes.Account>(model);
		}

		public void SaveAccount(Deano.Data.User model)
		{
			data.SaveAccount(model);
		}

		public void SaveCredentials(Deano.Data.User model)
		{
			model.Password = Crypto.Encrypt(model.Password);
			data.SaveCredentials(model);
		}

		public IEnumerable<prototypes.Forum> GetForumTopics()
		{
			var models = data.GetForumTopics();
			return ForumTransformation.Transform(models, string.Empty);
		}

		public IEnumerable<prototypes.Month> GetReportTopics()
		{
			int currentMonth = DateTime.Now.Month;
			int yearId = DateTime.Now.Year;
			DateTimeFormatInfo d = new DateTimeFormatInfo();
			List<Month> months = new List<Month>();
			for (int i = currentMonth; i > currentMonth - 12; i--)
			{
				int monthId = i;
				if (i <= 0)
				{
					monthId = 12 + i;
					Month month = new Month() { Name = d.GetMonthName(monthId), MonthId = monthId, YearId = yearId - 1 };
					months.Add(month);
				}
				else
				{
					Month month = new Month() { Name = d.GetMonthName(monthId), MonthId = monthId, YearId = yearId };
					months.Add(month);
				}
			}
			return months;
		}

		public void SaveForumTopic(Deano.Data.Forum model)
		{
			//var user = data.GetUser(HttpContext.Current.User.Identity.Name);
			//if (user != null)
			//{
			//	model.CreatedBy = user.Id;
			//}
			//else
			//{
			//	model.CreatedBy = 2;
			//}
			//model.CreatedDate = DateTime.Now;
			//data.SaveForumTopic(model);
		}

		public void SaveThread(prototypes.Thread model)
		{
			var user = data.GetUser(HttpContext.Current.User.Identity.Name);
			string createdBy = "1";
			if (user != null)
			{
				createdBy = user.Id;
			}

			//Deano.Data.Thread thread = Deano.Data.Thread.CreateThread(-1, model.Subject, model.ForumId, createdBy, DateTime.Now);
			//data.SaveThread(thread);
			//if (thread.ThreadId > 0)
			//{
			//	Deano.Data.Post post = Deano.Data.Post.CreatePost(-1, model.Body, createdBy, DateTime.Now, thread.ThreadId, new Guid(model.TemporaryId));
			//	post.Subject = thread.Subject;
			//	SavePost(post);
			//}
		}

		public IEnumerable<string> GetTags(int articleTypeId)
		{
			IEnumerable<string> tags = null;
			switch (articleTypeId)
			{
				case ArticleTypes.Post:
					tags = data.GetPostTags();
					break;
				case ArticleTypes.Report:
					tags = data.GetReportTags();
					break;
			}

			return tags;
		}

		public IQueryable<Deano.Data.Tag> GetTags()
		{
			return data.GetTags().Take(50);
		}

		public void SaveReport(Deano.Data.Report model)
		{
			var user = data.GetUser(HttpContext.Current.User.Identity.Name);
			//if (user != null)
			//{
			//	model.CreatedBy = user.UserId;
			//}
			//else
			//{
			//	model.CreatedBy = 2;
			//}
			//model.CreatedDate = DateTime.UtcNow;
			//int reportId = data.SaveReport(model, HttpContext.Current.User.Identity.Name);
			//SaveTags(reportId, model.Tags, ArticleTypes.Report);
		}

		public void SaveTags(int articleId, string tags, int articleTypeId)
		{
			if (!string.IsNullOrWhiteSpace(tags))
			{
				List<string> articleTags = tags.Split(new string[] { " ", ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
				if (articleTags != null && articleTags.Count > 0)
				{
					switch (articleTypeId)
					{
						case ArticleTypes.Post:
							data.DeleteArticleTags(articleId, null);
							data.SaveArticleTags(articleId, null, articleTags, ArticleTypes.Post);
							break;
						case ArticleTypes.Report:
							data.DeleteArticleTags(null, articleId);
							data.SaveArticleTags(null, articleId, articleTags, ArticleTypes.Report);
							break;
					}
				}
			}
		}

		public void SaveReportPictures(string temporaryId, List<string> paths)
		{
			data.SaveReportPictures(temporaryId, paths);
		}

		public IEnumerable<prototypes.Report> GetReportArchive(int? reportId, out int previousId, out int nextId)
		{
			previousId = 0;
			nextId = 0;
			var reports = data.GetReports(string.Empty, HttpContext.Current.User.Identity.Name);
			return ReportTransformation.Transform(reports.AsQueryable(), null, null, string.Empty).ToList();


			//prototypes.Report post = ReportTransformation.Transform<prototypes.Report>(data.GetReportArchive(reportId, out previousId, out nextId));
			//return post;
		}

		public IEnumerable<prototypes.Post> GetPostArchive(int? postId, out int previousId, out int nextId)
		{
			previousId = 0;
			nextId = 0;
			IEnumerable<Azure.Models.Post> posts = null;
			var thread = data.GetThreadPosts(-1, string.Empty, HttpContext.Current.User.Identity.Name, out posts);
			return PostTransformation.Transform(posts.AsQueryable(), string.Empty).ToList();

			//prototypes.Post post = PostTransformation.Transform<prototypes.Post>(data.GetPostArchive(postId, out previousId, out nextId));
			//return post;
		}

		public prototypes.Post GetPost(int postId)
		{
			return PostTransformation.Transform<prototypes.Post>(data.GetPost(postId));
		}

		public void SavePost(Deano.Data.Post model)
		{
			var user = data.GetUser(HttpContext.Current.User.Identity.Name);
			//if (user != null)
			//{
			//	model.CreatedBy = user.UserId;
			//}
			//else
			//{
			//	model.CreatedBy = 2;
			//}
			//model.CreatedDate = DateTime.Now;
			//int postId = data.SavePost(model);
			//SaveTags(postId, model.Tags, ArticleTypes.Post);
		}

		public void SavePostPictures(string temporaryId, List<string> paths)
		{
			data.SavePostPictures(temporaryId, paths);
		}

		public Catalog<prototypes.Thread, prototypes.Post> GetPostCatalog(int threadId, string search, int? firstIndex, int? selectedIndex)
		{
			IEnumerable<Azure.Models.Post> posts = null;
			int pageSize = int.Parse(data.GetSetting(Settings.PageSizePosts).Value);

			Catalog<prototypes.Thread, prototypes.Post> catalog = new Catalog<prototypes.Thread, prototypes.Post>();
			var thread = data.GetThreadPosts(threadId, search, HttpContext.Current.User.Identity.Name, out posts);
			catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), posts.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			catalog.Header = new prototypes.Thread() { ThreadId = threadId, Subject = thread.Subject };
			catalog.Items = PostTransformation.Transform(posts.AsQueryable(), string.Empty).ToList();
			catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			catalog.Header.Posts = catalog.Items;

			catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
			catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
			catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);

			if (search.Equals("My", StringComparison.InvariantCultureIgnoreCase))
			{
				catalog.Items.ForEach(m => m.AllowEdit = true);
			}
			else if (search.Equals("Favorites", StringComparison.InvariantCultureIgnoreCase))
			{
				catalog.Items.ForEach(m => m.Favorite = true);
			}
			return catalog;
		}

		public Catalog<prototypes.Forum, prototypes.Thread> GetForumCatalog(int forumId, int? firstIndex, int? selectedIndex)
		{
			int pageSize = int.Parse(data.GetSetting(Settings.PageSizeThreads).Value);
			Catalog<prototypes.Forum, prototypes.Thread> catalog = new Catalog<prototypes.Forum, prototypes.Thread>();

			IEnumerable<Azure.Models.Thread> threads;
			var forum = data.GetForumThreads(forumId, out threads);
			catalog.Header = new prototypes.Forum() { ForumId = forumId, Subject = forum.Subject };
			catalog.Items = ThreadTransformation.Transform(threads.AsQueryable(), string.Empty).ToList();
			catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			catalog.Header.Threads = catalog.Items;
			return catalog;
		}

		public Catalog<prototypes.Library, prototypes.Report> GetLibraryCatalog(string search, int? yearId, int? monthId, int? firstIndex, int? selectedIndex)
		{
			int year = yearId.HasValue ? yearId.Value : DateTime.Now.Year;
			int pageSize = int.Parse(data.GetSetting(Settings.PageSizeReports).Value);

			Catalog<prototypes.Library, prototypes.Report> catalog = new Catalog<prototypes.Library, prototypes.Report>();
			catalog.Header = new Library() { LibraryId = year, Title = "Library" };

			var reports = data.GetReports(search, HttpContext.Current.User.Identity.Name);
			catalog.Items = ReportTransformation.Transform(reports.AsQueryable(), yearId, monthId, string.Empty).ToList();
			catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
			catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
			catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);
			catalog.Header.Reports = catalog.Items;

			if (search.Equals("My", StringComparison.InvariantCultureIgnoreCase))
			{
				//catalog.Items.ForEach(m => m.AllowEdit = true);
			}
			else if (search.Equals("Favorites", StringComparison.InvariantCultureIgnoreCase))
			{
				catalog.Items.ForEach(m => m.Favorite = true);
			}
			return catalog;
		}

		public Catalog<prototypes.Album, prototypes.Photo> GetAlbum(string name, string search, int? firstIndex, int? selectedIndex)
		{
			Catalog<prototypes.Album, prototypes.Photo> catalog = new Catalog<prototypes.Album, prototypes.Photo>();
			List<prototypes.Photo> items = new List<prototypes.Photo>();

			//string username = picasaUserName; //data.GetSetting(Settings.PicasaUsername).Value;
			string accessToken = string.Empty;
			var service = GetPicasaService(out accessToken);
			//PicasaFeed feed = null;

			catalog.Header = new prototypes.Album() { AlbumId = name, Title = name };
			if (name.Equals("Recent", StringComparison.CurrentCultureIgnoreCase))
			{
				try
				{
					//PhotoQuery photoQuery = new PhotoQuery(PicasaQuery.CreatePicasaUri(username));
					//photoQuery.NumberToRetrieve = 10;
					//photoQuery.StartIndex = 1;
					//feed = service.Query(photoQuery);
					//foreach (PicasaEntry entry in feed.Entries)
					//{
					//	items.Add(new Photo() { PhotoId = entry.Id.Uri.Content, Thumbnail = entry.Media.Thumbnails[0].Attributes["url"] as string, Image = entry.Media.Content.Attributes["url"] as string });
					//}


					var result = service.GetAsync("https://photoslibrary.googleapis.com/v1/mediaItems?pageSize=10").Result;
					var content = result.Content.ReadAsStringAsync().Result;
					var photos = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchMediaItemsResponse>(content);
					foreach (var photo in photos.MediaItems)
					{
						string url = photo.BaseUrl;
						items.Add(new prototypes.Photo() { PhotoId = photo.Id, Thumbnail = url, Image = url });
					}
				}
				catch (Exception ex)
				{
					string s = ex.Message;
				}
			}
			//else if (name.Equals("-1") && !string.IsNullOrWhiteSpace(search) && !search.Equals("undefined", StringComparison.OrdinalIgnoreCase))
			//{
			//try
			//{
			//	PhotoQuery photoQuery = new PhotoQuery(PicasaQuery.CreatePicasaUri(username));
			//	photoQuery.Tags = search.Replace(" ", ",");
			//	photoQuery.KindParameter = "photo";
			//	feed = service.Query(photoQuery);
			//	foreach (PicasaEntry entry in feed.Entries)
			//	{
			//		items.Add(new Photo() { PhotoId = entry.Id.Uri.Content, Thumbnail = entry.Media.Thumbnails[0].Attributes["url"] as string, Image = entry.Media.Content.Attributes["url"] as string });
			//	}
			//}
			//catch (Exception ex)
			//{

			//}
			//}
			else
			{

				try
				{
					var album = GetAlbum(name);

					var request = new
					{
						albumId = album.AlbumId,
						pageSize = 10
					};

					var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
					var result = service.PostAsync("https://photoslibrary.googleapis.com/v1/mediaItems:search", requestContent).Result;
					var content = result.Content.ReadAsStringAsync().Result;
					var photos = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchMediaItemsResponse>(content);

					foreach (var photo in photos.MediaItems)
					{
						string url = photo.BaseUrl; //photo.ProductUrl + "?access_token=" + accessToken;
						items.Add(new prototypes.Photo() { PhotoId = photo.Id, Thumbnail = url, Image = url });
						//string contentUrl = entryPhoto.Media.Content.Attributes["url"] as string;
						//string firstThumbUrl = entryPhoto.Media.Thumbnails[0].Attributes["url"] as string;
					}
				}
				catch (Exception ex)
				{

				}


				//AlbumQuery query = new AlbumQuery(PicasaQuery.CreatePicasaUri(username));
				//query.Access = PicasaQuery.AccessLevel.AccessPublic;
				//feed = service.Query(query);
				//foreach (PicasaEntry entry in feed.Entries)
				//{
				//	if (entry.Title.Text.Equals(name))
				//	{
				//		AlbumAccessor ac = new AlbumAccessor(entry);
				//		PhotoQuery queryPhoto = new PhotoQuery(PicasaQuery.CreatePicasaUri(username, ac.Id));
				//		PicasaFeed feedPhoto = service.Query(queryPhoto);
				//		catalog.Header = new Album() { AlbumId = ac.Id, Title = ac.AlbumTitle };

				//		foreach (PicasaEntry entryPhoto in feedPhoto.Entries)
				//		{
				//			items.Add(new Photo() { PhotoId = entryPhoto.Id.Uri.Content, Thumbnail = entryPhoto.Media.Thumbnails[0].Attributes["url"] as string, Image = entryPhoto.Media.Content.Attributes["url"] as string });
				//			string contentUrl = entryPhoto.Media.Content.Attributes["url"] as string;
				//			string firstThumbUrl = entryPhoto.Media.Thumbnails[0].Attributes["url"] as string;
				//		}
				//	}
				//}
			}

			//todo set album page size;
			int albumPageSize = 10; //int.Parse(data.GetSetting(Settings.PageSizeAlbums).Value);
			catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), items.Count, selectedIndex.GetValueOrDefault(), albumPageSize);
			catalog.Items = items;
			catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			catalog.Header.Photos = catalog.Items;
			return catalog;
		}

		public IEnumerable<string> GetAlbumTags()
		{
			string username = picasaUserName;// data.GetSetting(Settings.PicasaUsername).Value;
			string accessToken = string.Empty;
			var service = GetPicasaService(out accessToken);
			List<string> tags = new List<string>();


			try
			{
				var result = service.GetAsync("https://photoslibrary.googleapis.com/v1/sharedAlbums").Result;
				var content = result.Content.ReadAsStringAsync().Result;
				var albums = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSharedAlbumsResponse>(content);
				foreach (var album in albums.SharedAlbums)
				{
					tags.Add(album.Title);
				}
			}
			catch (Exception ex)
			{

			}

			return tags;
		}
		public prototypes.Album GetAlbum(string name)
		{
			string username = picasaUserName;//data.GetSetting(Settings.PicasaUsername).Value;

			AlbumQuery query = new AlbumQuery(PicasaQuery.CreatePicasaUri(username));
			query.Access = PicasaQuery.AccessLevel.AccessPublic;
			string accessToken = string.Empty;
			var service = GetPicasaService(out accessToken);

			List<prototypes.Album> albums = new List<prototypes.Album>();

			var result = service.GetAsync("https://photoslibrary.googleapis.com/v1/sharedAlbums").Result;
			var content = result.Content.ReadAsStringAsync().Result;
			var albumsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSharedAlbumsResponse>(content);
			foreach (var albumResponse in albumsResponse.SharedAlbums)
			{
				prototypes.Album album = new prototypes.Album() { AlbumId = albumResponse.Id.ToString(), Title = albumResponse.Title };
				if (album.Title.Equals(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return album;
				}
			}

			return null;
		}

		public IEnumerable<prototypes.Album> GetAlbums()
		{
			string username = picasaUserName;//data.GetSetting(Settings.PicasaUsername).Value;

			AlbumQuery query = new AlbumQuery(PicasaQuery.CreatePicasaUri(username));
			query.Access = PicasaQuery.AccessLevel.AccessPublic;
			string accessToken = string.Empty;
			var service = GetPicasaService(out accessToken);

			List<prototypes.Album> albums = new List<prototypes.Album>();

			var result = service.GetAsync("https://photoslibrary.googleapis.com/v1/sharedAlbums").Result;
			var content = result.Content.ReadAsStringAsync().Result;
			var albumsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSharedAlbumsResponse>(content);
			foreach (var albumResponse in albumsResponse.SharedAlbums)
			{
				prototypes.Album album = new prototypes.Album() { AlbumId = albumResponse.Id.ToString(), Title = albumResponse.Title };
				albums.Add(album);
			}

			return albums;
		}

		PhotosLibraryService service = null;
		private static object serviceLock = new object();
		private HttpClient _client;
		private readonly object entry;

		private HttpClient GetPicasaService(out string accessToken)
		{
			//ServiceAccountCredential credential = ServiceAccountCredential.(new FileStream("MyProject-1234.json", FileMode.Open));
			//PrivateKey privateKey = credential.();
			//String privateKeyId = credential.getServiceAccountPrivateKeyId();

			lock (serviceLock)
			{
				if (_client != null)
				{
					accessToken = string.Empty;
					return _client;
				}



				//string username = picasaUserName;//data.GetSetting(Settings.PicasaUsername).Value;
				//string password = "fishWalleye"; //data.GetSetting(Settings.PicasaPassword).Value;

				string applicationName = "deanoswebapplication"; //data.GetSetting(Settings.PicasaApplicationName).Value;


				service = new PhotosLibraryService(); //(applicationName);
													  //service.setUserCredentials(username, password);

				string serviceAccountEmail = "deano-131@api-project-846631412293.deanosguideservice.com.iam.gserviceaccount.com";
				string certificatePath = System.Web.HttpContext.Current.Server.MapPath(@"\Shared\API Project-6268c4278944.p12");

				byte[] bytes = null;
				using (FileStream p12File = new FileStream(certificatePath, FileMode.Open))
				{
					using (MemoryStream ms = new MemoryStream())
					{
						p12File.CopyTo(ms);
						bytes = ms.ToArray();
						ms.Dispose();
					}
				}
				if (bytes == null || bytes.Length == 0)
				{
					throw new ApplicationException("Could not find file " + certificatePath);
				}
				var certificate = new X509Certificate2(bytes, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
				var serviceAccountCredential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
				{
					Scopes = new string[]
					{ "https://www.googleapis.com/auth/photoslibrary.readonly",
					"https://www.googleapis.com/auth/photoslibrary.sharing"
					}, // "https://picasaweb.google.com/data/" },
					User = "admin@deanosguideservice.com"

				}.FromCertificate(certificate));

				serviceAccountCredential.RequestAccessTokenAsync(System.Threading.CancellationToken.None).Wait();
				accessToken = serviceAccountCredential.Token.AccessToken;
				//accessToken = "ya29.sQIh2g8Mp_TheMPqm0t2Kk0iqrqHEtrfB5Ne7N8Jat2TxkagpDEiHGPoBYIGYaz61g";
				var requestFactory = new GDataRequestFactory("Deano");
				requestFactory.CustomHeaders.Add(string.Format("Authorization: Bearer {0}", accessToken));
				_client = new HttpClient();
				_client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));

				return _client;
			}
		}
	}
}
