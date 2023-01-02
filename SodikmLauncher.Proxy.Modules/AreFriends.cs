using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class AreFriends : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		if (!absolutePath.StartsWith("/game/arefriends"))
		{
			return absolutePath.StartsWith("/friend/arefriends");
		}
		return true;
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		string text = nameValueCollection["userId"];
		string text2 = nameValueCollection["otherUserIds"];
		if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
		{
			e.GenericResponse("", HttpStatusCode.BadRequest);
			return Task.CompletedTask;
		}
		long user = long.Parse(text);
		long[] users = text2.Split(',').Select(long.Parse).ToArray();
		long[] array = Friends.AreFriends(user, users);
		string text3 = "S";
		if (array.Length != 0)
		{
			text3 += string.Join(',', array);
			text3 += ",";
		}
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", text3.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(text3, headers);
		return Task.CompletedTask;
	}
}
