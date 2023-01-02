using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class JoinScript : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/join.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		string path = "./data/clients/" + ClientSettings.Instance.Year + "/game/join.lua";
		string text = "Failed to find Join for year " + ClientSettings.Instance.Year;
		if (File.Exists(path))
		{
			text = Crypto.ScriptSign(File.ReadAllText(path).Replace("{username}", Global.Customisation.Settings.Username).Replace("{ip}", Global.IP)
				.Replace("{port}", Global.Port.ToString())
				.Replace("{id}", Global.Customisation.Settings.Id.ToString())
				.Replace("{charapp}", Global.Customisation.ConstructCharappUrl()), 0L);
		}
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", text.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(text, headers);
		return Task.CompletedTask;
	}
}
