using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SodikmLauncher;

internal class Asset
{
	private static string XCsrfToken = "";

	public static AssetDeliveryBatchRequest[] ConstructBatchRequest(long[] ids)
	{
		List<AssetDeliveryBatchRequest> request = new List<AssetDeliveryBatchRequest>();
		ids.Distinct().ToList().ForEach(delegate(long x)
		{
			request.Add(new AssetDeliveryBatchRequest
			{
				assetId = x,
				requestId = x
			});
		});
		return request.ToArray();
	}

	public static async Task<AssetInformation[]> BatchRequest(AssetDeliveryBatchRequest[] request)
	{
		string strReq = JsonSerializer.Serialize(request);
		for (int i = 0; i < 5; i++)
		{
			HttpRequestMessage request2 = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://assetdelivery.roblox.com/v2/assets/batch"),
				Headers = 
				{
					{ "user-agent", "Roblox/WinInet" },
					{ "x-csrf-token", XCsrfToken }
				},
				Content = new StringContent(strReq, Encoding.UTF8, "application/json")
			};
			HttpResponseMessage httpResponseMessage = await Global.HttpClient.SendAsync(request2);
			if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
			{
				if (!httpResponseMessage.Headers.TryGetValues("x-csrf-token", out IEnumerable<string> values))
				{
					return new AssetInformation[0];
				}
				XCsrfToken = values.First();
				continue;
			}
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				return new AssetInformation[0];
			}
			return JsonSerializer.Deserialize<AssetInformation[]>(await httpResponseMessage.Content.ReadAsStringAsync()) ?? new AssetInformation[0];
		}
		return new AssetInformation[0];
	}

	public static async Task<AssetInformation[]> BatchRequest(long[] ids)
	{
		return await BatchRequest(ConstructBatchRequest(ids));
	}
}
