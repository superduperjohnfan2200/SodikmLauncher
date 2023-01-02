using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class PersistenceGet : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/persistence/getbloburl.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		if (string.IsNullOrEmpty(nameValueCollection["userid"]) || !long.TryParse(nameValueCollection["userid"], out var result))
		{
			e.GenericResponse("", HttpStatusCode.BadRequest);
			return Task.CompletedTask;
		}
		byte[] array = GameService.Instance.LoadBlob(result);
		e.Ok(array, new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "application/octet-stream"),
			new HttpHeader("Content-Length", array.LongLength.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		});
		return Task.CompletedTask;
	}
}
