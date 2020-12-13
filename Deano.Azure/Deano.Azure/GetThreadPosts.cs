using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace Deano.Azure
{
	public static class GetThreadPosts
	{
		[FunctionName("GetThreadPosts")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Threads/{id}/Posts")] HttpRequest req,
			ILogger log, string id)
		{

			int defaultPageSize = 4;
			int pageSize = 4;
			int pageIndex = 0;
			int defaultPageIndex = 0;

			if (req.Query != null)
			{
				var parameters = req.Query;
				if (parameters.TryGetValue("pageSize", out StringValues pageSizeValue))
				{
					if (!int.TryParse(pageSizeValue, out pageSize))
					{
						pageSize = defaultPageSize;
					}
				}
				if (parameters.TryGetValue("pageIndex", out StringValues pageIndexValue))
				{
					if (!int.TryParse(pageIndexValue, out pageIndex))
					{
						pageIndex = defaultPageIndex;
					}
				}
			}
			try
			{
				Storage storage = new Storage();
				var results = await storage.GetPosts(id, pageIndex, pageSize);
				return new OkObjectResult(results);
			}
			catch (Exception ex)
			{
				return new OkObjectResult(ex.Message);
			}
		}
	}
}
