using Deano.Azure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Deano.Azure.Models
{
	public static class Extensions
	{
		public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T value)
		{
			var jsonString = JsonConvert.SerializeObject(value, new TimeSpanConverter());
			var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
			return httpClient.PostAsync(requestUri, stringContent);

		}
		public static async Task<T> Deserialize<T>(this HttpRequestMessage httpRequest)
		{
			if (httpRequest.Content != null)
			{
				string requestBody = await httpRequest.Content.ReadAsStringAsync();
				if (!string.IsNullOrWhiteSpace(requestBody))
				{
					var result = JsonConvert.DeserializeObject<T>(requestBody, new TimeSpanConverter());
					return result;
				}
			}
			return default(T);
		}
	}
}
