using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class Studio : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.EndsWith("/game/studio.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		string path = "./data/clients/" + ClientSettings.Instance.Year + "/game/studio.lua";
		string text = ((!File.Exists(path)) ? ("Failed to find Studio for year " + ClientSettings.Instance.Year) : Crypto.ScriptSign(File.ReadAllText(path), 0L));
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
