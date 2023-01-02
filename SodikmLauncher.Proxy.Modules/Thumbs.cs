using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;

namespace SodikmLauncher.Proxy.Modules;

internal class Thumbs : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string path)
	{
		if (!absolutePath.StartsWith("/thumbs/asset.ashx"))
		{
			return absolutePath.StartsWith("/game/tools/thumbnailasset.ashx");
		}
		return true;
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection query = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		long id = 0L;
		if (long.TryParse(query["assetversionid"], out var result))
		{
			if ((await Global.HttpClient.GetAsync($"https://assetdelivery.roblox.com/v2/asset/?assetversionid={result}")).Content.Headers.TryGetValues("roblox-assetid", out IEnumerable<string> values))
			{
				id = long.Parse(values.First());
			}
		}
		else
		{
			long.TryParse(query["assetid"] ?? query["aid"], out id);
		}
		string format = query["format"] ?? "Png";
		string y = query["height"] ?? query["ht"] ?? query["y"] ?? "30";
		string x = query["width"] ?? query["wd"] ?? query["x"] ?? "30";
		e.Ok(await Thumbnails.GetThumbnail(id, x, y, format));
	}
}
