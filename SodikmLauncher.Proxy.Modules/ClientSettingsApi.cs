using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class ClientSettingsApi : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/setting/quietget/");
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		string path = "./data/clients/" + ClientSettings.Instance.Year + "/Flags.json";
		string text = "{}";
		if (File.Exists(path))
		{
			text = await File.ReadAllTextAsync(path);
		}
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "application/json"),
			new HttpHeader("Content-Length", text.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(text, headers);
	}
}
