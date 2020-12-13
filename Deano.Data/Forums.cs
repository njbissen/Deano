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
		string deanoListenersForums = "https://deanos-listeners.azurewebsites.net/api/forums";

		string deanoListenersThreads = "https://deanos-listeners.azurewebsites.net/api/threads";
		HttpClient httpClientForums = new HttpClient();

		public Post GetPostArchive(int? postId, out int previousId, out int nextId)
		{
			Post post = null;
			if (!postId.HasValue || postId.GetValueOrDefault() < 0)
			{
				post = context.Posts.FirstOrDefault();
				postId = post != null ? post.PostId : -1;
			}
			else
			{
				post = context.Posts.FirstOrDefault(m => m.PostId == postId.Value);
			}
			Post previous = context.Posts.Where(m => m.PostId < postId).OrderByDescending(m => m.PostId).FirstOrDefault();
			Post next = context.Posts.Where(m => m.PostId > postId.Value).FirstOrDefault();

			nextId = next != null ? next.PostId : -1;
			previousId = previous != null ? previous.PostId : -1;

			return post;
		}

		public IQueryable<Deano.Azure.Models.Forum> GetForumTopics()
		{
			//return context.Forums.OrderBy(m => m.Subject);


			var response = httpClientReports.GetAsync(deanoListenersForums).Result;
			var stringContent = response.Content.ReadAsStringAsync().Result;
			var reports = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.Forum>>(stringContent);

			return reports.AsQueryable();
		}

		public Azure.Models.Forum GetForumThreads(int forumId, out IEnumerable<Azure.Models.Thread> threads)
		{
			//return context.Forums.FirstOrDefault(m => m.ForumId == forumId);

			var response = httpClientReports.GetAsync($"{deanoListenersForums}/1/Threads").Result;
			var stringContent = response.Content.ReadAsStringAsync().Result;
			threads = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.Thread>>(stringContent);
			return GetForumTopics().FirstOrDefault();
		}

		public Post GetPost(int postId)
		{
			return context.Posts.FirstOrDefault(m => m.PostId == postId);
		}

		public Azure.Models.Thread GetThreadPosts(int threadId, string search, string username, out IEnumerable<Azure.Models.Post> posts)
		{
			var response = httpClientReports.GetAsync($"{deanoListenersForums}/1/Threads").Result;
			var stringContent = response.Content.ReadAsStringAsync().Result;
			var threads = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.Thread>>(stringContent);

			var postsResponse = httpClientReports.GetAsync($"{deanoListenersThreads}/1/Posts").Result;

			var stringContentPosts = postsResponse.Content.ReadAsStringAsync().Result;
			posts = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.Post>>(stringContentPosts);
			return threads.FirstOrDefault();


			//Thread model = null;
			//if (threadId > 0)
			//{
			//	model = context.Threads.FirstOrDefault(m => m.ThreadId == threadId);
			//	posts = model.Posts;
			//}
			//else
			//{
			//	model = Thread.CreateThread(-1, search, -1, -1, DateTime.Now);
			//	int userId = string.IsNullOrWhiteSpace(username) ? -1 : GetUser(username).UserId;
			//	if (search.Equals("My", StringComparison.InvariantCultureIgnoreCase))
			//	{
			//		posts = context.Posts.Where(m => m.CreatedBy == userId).OrderByDescending(m => m.CreatedDate);
			//	}
			//	else if (search.Equals("Favorites", StringComparison.InvariantCultureIgnoreCase))
			//	{
			//		IQueryable<Bookmark> bookmarks = context.Bookmarks.Where(m => m.PostId.HasValue && m.UserId == userId);
			//		posts = from p in context.Posts
			//				join b in bookmarks on p.PostId equals b.PostId.Value
			//				orderby p.CreatedDate descending
			//				select p;
			//	}
			//	else if (search.Equals("Recent", StringComparison.InvariantCultureIgnoreCase))
			//	{
			//		posts = context.Posts.OrderByDescending(m => m.CreatedDate);
			//	}
			//	else if (!string.IsNullOrWhiteSpace(search))
			//	{
			//		posts = context.Posts.Where(m => m.User.Handle.Contains(search)
			//				|| m.Subject.Contains(search)
			//				|| m.Body.Contains(search)
			//				|| m.Tags.Contains(search)).OrderByDescending(m => m.CreatedDate);
			//	}
			//	else
			//	{
			//		posts = context.Posts.OrderByDescending(m => m.CreatedDate);
			//	}
			//}
			//return model;
		}

		public void SaveForumTopic(Forum model)
		{
			if (model.ForumId > 0)
			{
				Forum forum = context.Forums.FirstOrDefault(m => m.ForumId == model.ForumId);
				forum.Subject = model.Subject;
				forum.Tags = model.Tags;
			}
			else
			{
				context.AddToForums(model);
			}
			context.SaveChanges();
		}

		public void SaveThread(Thread model)
		{
			if (model.ThreadId > 0)
			{
				Thread thread = context.Threads.FirstOrDefault(m => m.ThreadId == model.ThreadId);
				thread.Subject = model.Subject;
				thread.Tags = model.Tags;
			}
			else
			{
				context.AddToThreads(model);
			}
			context.SaveChanges();
		}

		public void SavePostPictures(string temporaryId, List<string> paths)
		{
			Guid id = new Guid(temporaryId);
			Post post = context.Posts.FirstOrDefault(m => m.TemporaryId.Equals(id));
			if (post != null)
			{
				foreach (string path in paths)
				{
					PostPicture postPicture = PostPicture.CreatePostPicture(-1, post.PostId, SavePhoto(path));
					context.AddToPostPictures(postPicture);
				}
				context.SaveChanges();
			}
		}

		public int SavePost(Post model)
		{
			if (model.PostId > 0)
			{
				Post post = context.Posts.FirstOrDefault(m => m.PostId == model.PostId);
				post.Subject = model.Subject;
				post.Body = model.Body;
				post.Tags = model.Tags;
				post.CreatedDate = DateTime.Now;
				context.SaveChanges();
			}
			else
			{
				context.AddToPosts(model);
				context.SaveChanges();

			}
			context.SaveChanges();
			return model.PostId;
		}

		public int SavePhoto(string path)
		{
			Picture model = Picture.CreatePicture(-1, path);
			context.AddToPictures(model);
			context.SaveChanges();
			return model.PictureId;
		}
	}
}
