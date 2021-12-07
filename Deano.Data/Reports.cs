using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Deano.Azure.Models;
using System.Net.Http.Headers;
using System.IO;

namespace Deano.Data
{
	public partial class Repository
	{

		string deanoListenersPhoto = "https://deanos-listeners.azurewebsites.net/api/photo";
		string deanoListenersReports = "https://deanos-listeners.azurewebsites.net/api/reports";
		HttpClient httpClientReports = new HttpClient();

		public Report GetReportArchive(int? reportId, out int previousId, out int nextId)
		{
			Report post = null;
			if (!reportId.HasValue || reportId.GetValueOrDefault() < 0)
			{
				post = context.Reports.FirstOrDefault();
				reportId = post != null ? post.ReportId : -1;
			}
			else
			{
				post = context.Reports.FirstOrDefault(m => m.ReportId == reportId.Value);
			}
			Report previous = context.Reports.Where(m => m.ReportId < reportId).OrderByDescending(m => m.ReportId).FirstOrDefault();
			Report next = context.Reports.Where(m => m.ReportId > reportId.Value).FirstOrDefault();

			nextId = next != null ? next.ReportId : -1;
			previousId = previous != null ? previous.ReportId : -1;

			return post;
		}

		public IQueryable<Deano.Azure.Models.Report> GetReports(string search, string username)
		{


			var response = httpClientReports.GetAsync(deanoListenersReports).Result;
			var stringContent = response.Content.ReadAsStringAsync().Result;
			var reports = JsonConvert.DeserializeObject<IEnumerable<Deano.Azure.Models.Report>>(stringContent);

			return reports.AsQueryable();

			//int userId = string.IsNullOrWhiteSpace(username) ? -1 : GetUser(username).UserId;

			//if (search.Equals("My", StringComparison.InvariantCultureIgnoreCase))
			//{
			//	reports = context.Reports.Where(m => m.CreatedBy == userId).OrderByDescending(m => m.CreatedDate);
			//}
			//else if (search.Equals("Favorites", StringComparison.InvariantCultureIgnoreCase))
			//{
			//	IQueryable<Bookmark> bookmarks = context.Bookmarks.Where(m => m.ReportId.HasValue && m.UserId == userId);
			//	reports = from p in context.Reports
			//			  join b in bookmarks on p.ReportId equals b.ReportId.Value
			//			  orderby p.CreatedDate descending
			//			  select p;
			//}
			//else if (search.Equals("Recent", StringComparison.InvariantCultureIgnoreCase))
			//{
			//	reports = context.Reports.OrderByDescending(m => m.CreatedDate);
			//}
			//else if (!string.IsNullOrWhiteSpace(search))
			//{
			//	reports = context.Reports.Where(m => m.User.Handle.Contains(search)
			//			|| m.Subject.Contains(search)
			//			|| m.Body.Contains(search)
			//			|| m.Tags.Contains(search)).OrderByDescending(m => m.CreatedDate);
			//}
			//else
			//{
			//	reports = context.Reports.OrderByDescending(m => m.CreatedDate);
			//}
			//return reports;
		}

		public int SaveReport(Report model, string username)
		{

			Azure.Models.Report report = new Azure.Models.Report()
			{
				Id = Guid.NewGuid().ToString(),
				Subject = model.Subject,
				Body = model.Body,
				CreatedBy = username,
				CreatedDate = model.CreatedDate,
				Tags = model.Tags
			};

			httpClientReports.PostAsJsonAsync(deanoListenersReports, report).Wait();
			return 1;

			//if (model.ReportId > 0)
			//{
			//	Report post = context.Reports.FirstOrDefault(m => m.ReportId == model.ReportId);
			//	post.Subject = model.Subject;
			//	post.Body = model.Body;
			//	post.Tags = model.Tags;
			//	context.SaveChanges();
			//}
			//else
			//{
			//	context.AddToReports(model);
			//	context.SaveChanges();
			//}
			//context.SaveChanges();
			//return model.ReportId;
		}

		public void SaveReportPictures(string temporaryId, List<string> paths)
		{
			string filePath = "";

			using (var form = new MultipartFormDataContent())
			{

				var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
				fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
				form.Add(fileContent, "file", Path.GetFileName(filePath));

				var response = httpClientReports.PostAsync(deanoListenersPhoto, form);
			}


			Guid id = new Guid(temporaryId);
			Report post = context.Reports.FirstOrDefault(m => m.TemporaryId.Equals(id));
			if (post != null)
			{
				foreach (string path in paths)
				{
					ReportPicture postPicture = ReportPicture.CreateReportPicture(-1, post.ReportId, SavePhoto(path));
					context.AddToReportPictures(postPicture);
				}
				context.SaveChanges();
			}
		}
	}
}
