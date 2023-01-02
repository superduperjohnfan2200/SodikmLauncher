using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class CharacterFetch : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/asset/characterfetch.ashx");
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		string text = HttpUtility.UrlDecode(HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query)["userId"]);
		if (string.IsNullOrEmpty(text))
		{
			e.GenericResponse("", HttpStatusCode.InternalServerError);
			return;
		}
		string[] array = text.Split(',');
		if (array.Length == 0)
		{
			e.GenericResponse("", HttpStatusCode.InternalServerError);
			return;
		}
		string data = "http://www.roblox.com/Asset/BodyColors.ashx?userId=" + HttpUtility.UrlEncode(array[0]);
		(await Global.Customisation.FilterWhitelisted(array[1..].Select(long.Parse).ToArray())).ToList().ForEach(delegate(long x)
		{
			data += $";http://www.roblox.com/Asset/?id={x}";
		});
		e.Ok(data, new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", data.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		});
	}
}
