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
	public static class SaveReport
	{
		[FunctionName("SaveReport")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Reports")] HttpRequest req,
			ILogger log)
		{


			try
			{
				Storage storage = new Storage();
				var report = await req.Deserialize<Report>();
				var result = await storage.SaveReport(report);
				return new OkObjectResult(result);
			}
			catch (Exception ex)
			{
				return new OkObjectResult(ex.Message);
			}
		}
	}
}
