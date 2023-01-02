using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class PersistenceSet : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/persistence/setblob.ashx");
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		if (string.IsNullOrEmpty(nameValueCollection["userid"]) || !long.TryParse(nameValueCollection["userid"], out var userId))
		{
			e.GenericResponse("", HttpStatusCode.BadRequest);
			return;
		}
		byte[] blob = await e.GetRequestBody();
		GameService.Instance.SaveBlob(userId, blob);
		e.Ok("", new List<HttpHeader>
		{
			new HttpHeader("Cache-Control", "no-cache")
		});
	}
}
