using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class LWSSocial : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/luawebservice/handlesocialrequest.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		bool flag = false;
		string text = nameValueCollection["method"];
		if (string.IsNullOrEmpty(text))
		{
			return Task.CompletedTask;
		}
		if (text == "IsFriendsWith")
		{
			string text2 = nameValueCollection["playerid"];
			string text3 = nameValueCollection["userid"];
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3) && long.TryParse(text2, out var result) && long.TryParse(text3, out var result2))
			{
				flag = result == result2 || Friends.AreFriend(result, result2);
			}
		}
		string text4 = "<Value Type=\"boolean\">" + flag.ToString().ToLowerInvariant() + "</Value>";
		e.GenericResponse(text4, HttpStatusCode.OK, new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/html"),
			new HttpHeader("Content-Length", text4.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		});
		return Task.CompletedTask;
	}
}
