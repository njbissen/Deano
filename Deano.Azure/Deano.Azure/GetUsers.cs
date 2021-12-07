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
	public static class GetUsers
	{
		[FunctionName("GetUsers")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Users")] HttpRequest req,
			ILogger log)
		{

			try
			{
				Storage storage = new Storage();
				var users = await storage.GetUsers();
				return new OkObjectResult(users);
			}
			catch (Exception ex)
			{
				return new OkObjectResult(ex.Message);
			}
		}
	}
}
