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
using Deano.Azure.Models;

namespace Deano.Azure
{
	public static class SaveForum
	{
		[FunctionName("SaveForum")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Forums")] HttpRequest req,
			ILogger log)
		{


			try
			{
				Storage storage = new Storage();
				var forum = await req.Deserialize<Forum>();
				var result = await storage.SaveForum(forum);
				return new OkObjectResult(result);
			}
			catch (Exception ex)
			{
				return new OkObjectResult(ex.Message);
			}
		}
	}
}
