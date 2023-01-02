using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class PlaceSpecificScript : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/placespecificscript.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query);
		string year = ClientSettings.Instance.Year;
		string path = "./data/clients/" + year + "/game/placespecificscript.lua";
		string text = ((!File.Exists(path)) ? ("Failed to find PlaceSpecificScript for year " + year) : Crypto.ScriptSign(File.ReadAllText(path).Replace("{id}", nameValueCollection["placeid"]), 0L));
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
