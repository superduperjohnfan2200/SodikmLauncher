using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class PlayerTracking : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/playertracking.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		string value = nameValueCollection["m"];
		string text = nameValueCollection["i"];
		string text2 = nameValueCollection["n"];
		if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text) || !long.TryParse(text, out var result))
		{
			e.GenericResponse("Missing or invalid parameters", HttpStatusCode.BadRequest);
			return Task.CompletedTask;
		}
		string text3 = nameValueCollection["m"];
		if (!(text3 == "u"))
		{
			if (!(text3 == "r"))
			{
				e.GenericResponse("Unknown mode", HttpStatusCode.BadRequest);
				return Task.CompletedTask;
			}
			if (string.IsNullOrEmpty(text2))
			{
				e.GenericResponse("Name is empty or null", HttpStatusCode.BadRequest);
				return Task.CompletedTask;
			}
			GameService.Instance.RegisterPlayer(result, text2);
		}
		else
		{
			GameService.Instance.UnregisterPlayer(result);
		}
		e.Ok("", new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Cache-Control", "no-cache")
		});
		return Task.CompletedTask;
	}
}
