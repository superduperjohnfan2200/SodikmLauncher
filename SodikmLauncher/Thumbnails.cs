using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SodikmLauncher;

internal class Thumbnails
{
	private static List<Tuple<int, int>> ValidThumbnailSize = new List<Tuple<int, int>>
	{
		new Tuple<int, int>(30, 30),
		new Tuple<int, int>(42, 42),
		new Tuple<int, int>(50, 50),
		new Tuple<int, int>(75, 75),
		new Tuple<int, int>(110, 110),
		new Tuple<int, int>(140, 140),
		new Tuple<int, int>(150, 150),
		new Tuple<int, int>(160, 100),
		new Tuple<int, int>(160, 600),
		new Tuple<int, int>(250, 250),
		new Tuple<int, int>(256, 144),
		new Tuple<int, int>(300, 250),
		new Tuple<int, int>(384, 216),
		new Tuple<int, int>(420, 420),
		new Tuple<int, int>(480, 270),
		new Tuple<int, int>(512, 512),
		new Tuple<int, int>(576, 324),
		new Tuple<int, int>(700, 700),
		new Tuple<int, int>(728, 90),
		new Tuple<int, int>(768, 432)
	};

	private static readonly byte[] DeletedImage = File.ReadAllBytes("./data/web/thumbs/deleted.png");

	public static string GetClosestSize(int x, int y)
	{
		Tuple<int, int> tuple = ValidThumbnailSize.OrderBy((Tuple<int, int> e) => Math.Abs(e.Item1 - x) + Math.Abs(e.Item2 - y)).FirstOrDefault();
		return $"{tuple?.Item1}x{tuple?.Item2}";
	}

	public static async Task<byte[]> GetThumbnail(long assetId, int x, int y, string format)
	{
		string closestSize = GetClosestSize(x, y);
		string post = $"[{{\"targetId\":{assetId},\"type\":\"Asset\",\"size\":\"{closestSize}\",\"format\":\"{format}\",\"isCircular\":false}}]";
		for (int i = 0; i < 5; i++)
		{
			HttpResponseMessage httpResponseMessage = await Global.HttpClient.PostAsync("https://thumbnails.roblox.com/v1/batch", new StringContent(post, Encoding.UTF8, "application/json"));
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if (httpResponseMessage.StatusCode != HttpStatusCode.TooManyRequests)
				{
					break;
				}
				Task.Delay(500).Wait();
				continue;
			}
			ThumbnailsAssets? thumbnailsAssets = JsonSerializer.Deserialize<ThumbnailsAssets>(await httpResponseMessage.Content.ReadAsStringAsync());
			object obj;
			if (thumbnailsAssets == null)
			{
				obj = null;
			}
			else
			{
				ThumbnailsAssetsData[]? data = thumbnailsAssets.data;
				obj = ((data != null) ? data[0] : null);
			}
			string requestUri = ((ThumbnailsAssetsData)obj)?.imageUrl;
			if (((ThumbnailsAssetsData)obj)?.state != "Completed")
			{
				break;
			}
			return await Global.HttpClient.GetByteArrayAsync(requestUri);
		}
		return DeletedImage;
	}

	public static async Task<byte[]> GetThumbnail(long assetId, string x, string y, string format)
	{
		return await GetThumbnail(assetId, int.Parse(x), int.Parse(y), format);
	}
}
