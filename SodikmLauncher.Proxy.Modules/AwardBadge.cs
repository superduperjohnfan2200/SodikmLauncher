using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class AwardBadge : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/badge/awardbadge.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		string text = GameService.Instance.AwardBadge(Convert.ToInt64(nameValueCollection["badgeid"]), Convert.ToInt64(nameValueCollection["userid"]));
		IEnumerable<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", text.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(text, headers);
		return Task.CompletedTask;
	}
}
