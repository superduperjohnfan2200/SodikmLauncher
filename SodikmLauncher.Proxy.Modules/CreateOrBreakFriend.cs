using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class CreateOrBreakFriend : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		if (!absolutePath.StartsWith("/game/createfriend") && !absolutePath.StartsWith("/friend/createfriend") && !absolutePath.StartsWith("/game/breakfriend"))
		{
			return absolutePath.StartsWith("/friend/breakfriend");
		}
		return true;
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		string text = nameValueCollection["firstUserId"];
		string text2 = nameValueCollection["secondUserId"];
		if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
		{
			e.GenericResponse("", HttpStatusCode.BadRequest);
			return Task.CompletedTask;
		}
		if (!long.TryParse(text, out var result) || !long.TryParse(text2, out var result2))
		{
			e.GenericResponse("", HttpStatusCode.BadRequest);
			return Task.CompletedTask;
		}
		string text3 = e.HttpClient.Request.RequestUri.AbsolutePath.ToLowerInvariant();
		if (text3.StartsWith("/game/createfriend") || text3.StartsWith("/friend/createfriend"))
		{
			Friends.CreateFriend(result, result2);
		}
		else if (text3.StartsWith("/game/breakfriend") || text3.StartsWith("/friend/breakfriend"))
		{
			Friends.BreakFriend(result, result2);
		}
		string html = "";
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(html, headers);
		return Task.CompletedTask;
	}
}
