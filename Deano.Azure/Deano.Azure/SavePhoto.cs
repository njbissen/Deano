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

namespace Deano.Azure
{
	public static class SavePhoto
	{
		[FunctionName("SavePhoto")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Photo")] HttpRequestMessage req,
			ILogger log)
		{

			var multipartMemoryStreamProvider = new MultipartMemoryStreamProvider();

			await req.Content.ReadAsMultipartAsync(multipartMemoryStreamProvider);

			var file = multipartMemoryStreamProvider.Contents[0];
			using (var fileStream = await file.ReadAsStreamAsync())
			{
				// await cloudBlockBlob.UploadFromStreamAsync(fileStream);
			}

			return new OkResult();
		}
	}
}
