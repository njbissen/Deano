using Deano.Azure.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Deano.Azure
{
	public static class Extensions
	{
		public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T value)
		{
			var jsonString = JsonConvert.SerializeObject(value, new TimeSpanConverter());
			var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
			return httpClient.PostAsync(requestUri, stringContent);

		}
		public static async Task<T> Deserialize<T>(this HttpRequest httpRequest)
		{
			if (httpRequest.Body != null)
			{
				using (StreamReader reader = new StreamReader(httpRequest.Body))
				{
					string requestBody = await reader.ReadToEndAsync();
					if (!string.IsNullOrWhiteSpace(requestBody))
					{
						var result = JsonConvert.DeserializeObject<T>(requestBody, new TimeSpanConverter());
						return result;
					}
				}
			}
			return default(T);
		}
	}
}
