using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Deano.Data
{
	public partial class Repository
	{

		string deanoListenersUsers = "https://deanos-listeners.azurewebsites.net/api/users";
		HttpClient httpClientUsers = new HttpClient();
		public Deano.Data.Database context = new Database();

		public Azure.Models.User GetUser(string username)
		{
			var response = httpClientReports.GetAsync(deanoListenersUsers).Result;
			var stringContent = response.Content.ReadAsStringAsync().Result;
			var users = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.User>>(stringContent);

			return users.FirstOrDefault(m => m.Name == username);
		}

		public void SaveCredentials(User model)
		{
			User user = context.Users.FirstOrDefault(m => m.UserId == model.UserId);
			if (user != null)
			{
				user.Username = model.Username;
				user.Password = model.Password;
				context.SaveChanges();
			}
		}

		public void SaveAccount(User model)
		{
			User user = context.Users.FirstOrDefault(m => m.UserId == model.UserId);
			if (user != null)
			{
				user.Handle = model.Handle;
				user.Phone = model.Phone;
				context.SaveChanges();
			}
		}

		public Azure.Models.User GetUser(string username, string password)
		{

			var response = httpClientReports.GetAsync(deanoListenersUsers).Result;
			var stringContent = response.Content.ReadAsStringAsync().Result;
			var users = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.User>>(stringContent);
			var user = users.FirstOrDefault(m => m.Name.Equals(username) && m.Password.Equals(password));
			return user;
		}

		public User GetUserByHandle(string handle)
		{
			User user = context.Users.FirstOrDefault(m => m.Handle.Equals(handle));
			return user;
		}

		public User SaveRegistration(Deano.Data.User model)
		{
			context.AddToUsers(model);
			context.SaveChanges();
			return model;
		}

		public void SaveMessage(Deano.Data.Message model)
		{
			//context.AddToMessages(model);
			//context.SaveChanges();
		}

		public void SaveBookmark(int userId, int? postId, int? reportId)
		{
			Bookmark bookmark = Bookmark.CreateBookmark(-1, userId);
			bookmark.PostId = postId;
			bookmark.ReportId = reportId;
			context.AddToBookmarks(bookmark);
			context.SaveChanges();
		}

		#region "Settings"

		public Setting GetSetting(int settingId)
		{
			//Setting setting = context.Settings.FirstOrDefault(m => m.SettingId == settingId);
			//if (setting != null && setting.Encrypted)
			//{
			//    setting.Value = Crypto.Decrypt(setting.Value);
			//}
			return new Setting()
			{
				Value = Settings[settingId]
			};
		}

		#endregion

		#region "MetaTags"

		public string GetMetaTag(int typeId, string name)
		{
			//todo return tags
			//MetaTag tag = context.MetaTags.Where(m => m.TypeId == typeId && m.Name.Equals(name)).FirstOrDefault();
			//         return tag != null ? tag.Value : string.Empty;
			return string.Empty;
		}

		#endregion

		#region "Tags"

		public IEnumerable<string> GetReportTags()
		{
			return new List<string>();
			//return context.ArticleTags.Where(m => m.ReportId.HasValue).Select(m => m.Tag.Name).AsEnumerable();
		}

		public IEnumerable<string> GetPostTags()
		{
			return new List<string>();

			//  return context.ArticleTags.Where(m => m.PostId.HasValue).Select(m => m.Tag.Name).AsEnumerable();
		}

		public IQueryable<Tag> GetTags()
		{
			// return context.Tags.OrderByDescending(m => m.TagId);
			//todo return tags;
			return (new List<Tag>() { new Tag() { Name = "tag", TagId = 1 } }).AsQueryable();
		}

		public void SaveArticleTags(int? postId, int? reportId, List<string> tags, int articleTypeId)
		{
			Tag existingTag = null;
			ArticleTag articleTag = null;
			foreach (string tag in tags)
			{
				existingTag = context.Tags.FirstOrDefault(m => m.Name.Equals(tag));
				if (existingTag == null)
				{
					existingTag = Tag.CreateTag(-1, tag.ToLower(), articleTypeId);
					context.AddToTags(existingTag);
					context.SaveChanges();
				}
				articleTag = ArticleTag.CreateArticleTag(-1, existingTag.TagId);
				if (postId.HasValue)
				{
					articleTag.PostId = postId.Value;
				}
				else if (reportId.HasValue)
				{
					articleTag.ReportId = reportId.Value;
				}
				context.AddToArticleTags(articleTag);
				context.SaveChanges();
			}
		}

		public void DeleteArticleTags(int? postId, int? reportId)
		{
			ArticleTag tag = null;
			List<int> tags = null;
			if (postId.HasValue)
			{
				tags = context.ArticleTags.Where(m => m.PostId == postId.Value).Select(m => m.ArticleTagId).ToList();

			}
			else if (reportId.HasValue)
			{
				tags = context.ArticleTags.Where(m => m.ReportId == postId.Value).Select(m => m.ArticleTagId).ToList();
			}

			if (tags != null)
			{
				for (int i = 0; i < tags.Count(); i++)
				{
					tag = context.ArticleTags.FirstOrDefault(m => m.ArticleTagId == tags[i]);
					if (tag != null)
					{
						context.DeleteObject(tag);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion

		private IDictionary<int, string> Settings = new Dictionary<int, string>()
		{
			{
				SettingKey.PageSizeReports, "4"
			},
			{
				SettingKey.PageSizeAlbums, "24"
			},
			{
				SettingKey.PageSizeThreads, "10"
			},
			{
				SettingKey.PageSizePosts, "5"
			},
			{
				SettingKey.NoReplyEmailAddress, "noreply@deanosguideservice.com"
			},
			{
				SettingKey.EmailCredentialsUsername, "admin@deanosguideservice.com"
			},
			{
				SettingKey.EmailCredentialsPassword, "fishWalleye"
			},
			{
				SettingKey.RequestEmailAddress, "admin@deanosguideservice.com;nicholas.bissen@gmail.com"
			},
			{
				SettingKey.RequestDisplayName, "deano"
			},
			{
				SettingKey.DomainName, "deanosguideservice.com"
			},
			{
				SettingKey.MapsKey, "AIzaSyBbaBQ_frVH9f3h5_vHEJGtnUJDFwtACQY"
			}
		};

		public static class SettingKey
		{
			public const int PageSizeReports = 1;
			public const int PageSizeAlbums = 3;
			public const int PageSizeThreads = 4;
			public const int PageSizePosts = 5;
			public const int NoReplyEmailAddress = 6;
			public const int EmailCredentialsUsername = 7;
			public const int EmailCredentialsPassword = 9;
			public const int RequestEmailAddress = 11;
			public const int RequestDisplayName = 12;
			public const int DomainName = 13;
			public const int MapsKey = 17;

		}
	}
}
