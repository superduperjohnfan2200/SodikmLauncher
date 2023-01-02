using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class Sets : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/tools/insertasset.ashx");
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		string text = "";
		long result2;
		if (long.TryParse(nameValueCollection["userid"], out var result))
		{
			text = $"./data/web/sets/u{result}.xml";
		}
		else if (long.TryParse(nameValueCollection["sid"], out result2))
		{
			text = $"./data/web/sets/s{result2}.xml";
		}
		else if (nameValueCollection["type"] == "base")
		{
			text = "./data/web/sets/base.xml";
		}
		string text2 = "<List></List>";
		if (!string.IsNullOrEmpty(text) && File.Exists(text))
		{
			text2 = await File.ReadAllTextAsync(text);
		}
		e.GenericResponse(text2, HttpStatusCode.OK, new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", text2.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		});
	}
}
