using Deano.Azure.Entities;
using Deano.Azure.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Deano.Azure
{
	public class Storage
	{
		private static string ReportsTableName = "reports";
		private static string ForumsTableName = "forumTopics";
		private static string ForumThreadsTableName = "forumThreads";
		private static string ForumPostsTableName = "forumPosts";

		public async Task<IEnumerable<Forum>> GetForums(int pageIndex, int pageSize)
		{
			IList<Forum> forums = new List<Forum>();
			var table = GetTable(ForumsTableName);
			TableQuery<ForumEntity> query = new TableQuery<ForumEntity>();

			TableContinuationToken token = null;
			do
			{
				var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
				token = resultSegment.ContinuationToken;

				foreach (var entity in resultSegment.Results)
				{
					forums.Add(new Forum()
					{
						Id = entity.RowKey,
						Subject = entity.Name,
						Tags = entity.Tags,
						CreatedBy = entity.CreatedBy,
						CreatedDate = entity.CreatedDate
					});
				}
			} while (token != null);

			return forums.OrderBy(m => m.CreatedDate).Skip(pageSize * pageIndex).Take(pageSize);

			// Catalog<prototypes.Library, prototypes.Report> catalog = new Catalog<prototypes.Library, prototypes.Report>();
			// catalog.Header = new Library() { LibraryId = 2020, Title = "Library" };

			// int? firstIndex = 0;
			// int? selectedIndex = 0;
			// int pageSize = 10;
			// //  IQueryable<Deano.Data.Report> reports = data.GetReports(search, HttpContext.Current.User.Identity.Name);
			// catalog.Items = reports.ToList(); // ReportTransformation.Transform(reports.AsQueryable(), yearId, monthId, string.Empty).ToList();
			// catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			// catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
			//// catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);
			// catalog.Header.Reports = catalog.Items;
			// return catalog;
		}

		public async Task<IEnumerable<Thread>> GetThreads(string forumId, int pageIndex, int pageSize)
		{
			IList<Thread> threads = new List<Thread>();
			var table = GetTable(ForumThreadsTableName);
			TableQuery<ThreadEntity> query = new TableQuery<ThreadEntity>();

			TableContinuationToken token = null;
			do
			{
				var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
				token = resultSegment.ContinuationToken;

				foreach (var entity in resultSegment.Results)
				{
					threads.Add(new Thread()
					{
						Id = entity.RowKey,
						Subject = entity.Name,
						Tags = entity.Tags,
						CreatedBy = entity.CreatedBy,
						CreatedDate = entity.CreatedDate
					});
				}
			} while (token != null);

			return threads.OrderBy(m => m.CreatedDate).Skip(pageSize * pageIndex).Take(pageSize);

			// Catalog<prototypes.Library, prototypes.Report> catalog = new Catalog<prototypes.Library, prototypes.Report>();
			// catalog.Header = new Library() { LibraryId = 2020, Title = "Library" };

			// int? firstIndex = 0;
			// int? selectedIndex = 0;
			// int pageSize = 10;
			// //  IQueryable<Deano.Data.Report> reports = data.GetReports(search, HttpContext.Current.User.Identity.Name);
			// catalog.Items = reports.ToList(); // ReportTransformation.Transform(reports.AsQueryable(), yearId, monthId, string.Empty).ToList();
			// catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			// catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
			//// catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);
			// catalog.Header.Reports = catalog.Items;
			// return catalog;
		}

		public async Task<IEnumerable<Post>> GetPosts(string threadId, int pageIndex, int pageSize)
		{
			IList<Post> posts = new List<Post>();
			var table = GetTable(ForumPostsTableName);
			TableQuery<PostEntity> query = new TableQuery<PostEntity>();

			TableContinuationToken token = null;
			do
			{
				var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
				token = resultSegment.ContinuationToken;

				foreach (var entity in resultSegment.Results)
				{
					posts.Add(new Post()
					{
						Id = entity.RowKey,
						Subject = entity.Content,
						Tags = entity.Tags,
						Body = entity.Body,
						CreatedBy = entity.CreatedBy,
						CreatedDate = entity.CreatedDate
					});
				}
			} while (token != null);

			return posts.OrderBy(m => m.CreatedDate).Skip(pageSize * pageIndex).Take(pageSize);

			// Catalog<prototypes.Library, prototypes.Report> catalog = new Catalog<prototypes.Library, prototypes.Report>();
			// catalog.Header = new Library() { LibraryId = 2020, Title = "Library" };

			// int? firstIndex = 0;
			// int? selectedIndex = 0;
			// int pageSize = 10;
			// //  IQueryable<Deano.Data.Report> reports = data.GetReports(search, HttpContext.Current.User.Identity.Name);
			// catalog.Items = reports.ToList(); // ReportTransformation.Transform(reports.AsQueryable(), yearId, monthId, string.Empty).ToList();
			// catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			// catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
			//// catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);
			// catalog.Header.Reports = catalog.Items;
			// return catalog;
		}

		public async Task<IEnumerable<Report>> GetReports(int pageIndex, int pageSize)
		{
			IList<Report> reports = new List<Report>();
			var table = GetTable(ReportsTableName);
			TableQuery<ReportEntity> query = new TableQuery<ReportEntity>();

			TableContinuationToken token = null;
			do
			{
				var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
				token = resultSegment.ContinuationToken;

				foreach (var entity in resultSegment.Results)
				{
					reports.Add(new Report()
					{
						Id = entity.RowKey,
						Subject = entity.Subject,
						Tags = entity.Tags,
						Body = entity.Body,
						CreatedBy = entity.CreatedBy,
						CreatedDate = entity.CreatedDate
					});
				}
			} while (token != null);

			return reports.OrderBy(m => m.CreatedDate).Skip(pageSize * pageIndex).Take(pageSize);

			// Catalog<prototypes.Library, prototypes.Report> catalog = new Catalog<prototypes.Library, prototypes.Report>();
			// catalog.Header = new Library() { LibraryId = 2020, Title = "Library" };

			// int? firstIndex = 0;
			// int? selectedIndex = 0;
			// int pageSize = 10;
			// //  IQueryable<Deano.Data.Report> reports = data.GetReports(search, HttpContext.Current.User.Identity.Name);
			// catalog.Items = reports.ToList(); // ReportTransformation.Transform(reports.AsQueryable(), yearId, monthId, string.Empty).ToList();
			// catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
			// catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
			//// catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
			//// catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);
			// catalog.Header.Reports = catalog.Items;
			// return catalog;
		}

		private CloudTable GetTable(string tableName)
		{
			string connectionString = Environment.GetEnvironmentVariable("Storage");
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
			var tableClient = storageAccount.CreateCloudTableClient();
			var table = tableClient.GetTableReference(tableName);
			return table;
		}

		public async Task<IEnumerable<Report>> GetReportsRecent(int pageIndex, int pageSize)
		{
			IList<Report> reports = new List<Report>();

			var table = GetTable(ReportsTableName);
			TableQuery<ReportEntity> query = new TableQuery<ReportEntity>();

			// Print the fields for each customer.
			TableContinuationToken token = null;
			do
			{
				var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
				token = resultSegment.ContinuationToken;

				foreach (var entity in resultSegment.Results)
				{
					reports.Add(new Report()
					{
						Id = entity.RowKey,
						Subject = entity.Subject,
						Body = entity.Body,
						CreatedBy = entity.CreatedBy,
						CreatedDate = entity.CreatedDate
					});
				}
			} while (token != null);

			return reports.OrderByDescending(m => m.CreatedDate).Skip(pageSize * pageIndex).Take(pageSize);
		}

		public async Task<string> SaveReport(Report report)
		{

			var table = GetTable(ReportsTableName);
			var entity = new ReportEntity()
			{
				RowKey = Guid.NewGuid().ToString(),
				PartitionKey = $"{report.CreatedDate.Year}-{report.CreatedDate.Month}",
				Subject = report.Subject,
				Body = report.Body,
				CreatedBy = report.CreatedBy,
				CreatedDate = report.CreatedDate,
				Tags = report.Tags
			};

			var operation = TableOperation.InsertOrReplace(entity);
			await table.ExecuteAsync(operation);
			return entity.RowKey;
		}

		public async Task<string> SaveForum(Forum forum)
		{

			var table = GetTable(ForumsTableName);
			var entity = new ForumEntity()
			{
				RowKey = Guid.NewGuid().ToString(),
				PartitionKey = $"{forum.CreatedDate.Year}-{forum.CreatedDate.Month}",
				Name = forum.Subject,
				CreatedBy = forum.CreatedBy,
				CreatedDate = forum.CreatedDate,
				Tags = forum.Tags
			};

			var operation = TableOperation.InsertOrReplace(entity);
			await table.ExecuteAsync(operation);
			return entity.RowKey;
		}
	}
}
