using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class GameServerScript : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/game/gameserver.ashx");
	}

	public static string GetScript()
	{
		string year = ClientSettings.Instance.Year;
		string path = "./data/clients/" + year + "/game/gameserver.lua";
		string result = "Failed to find GameServer for year " + year;
		if (File.Exists(path))
		{
			result = Crypto.ScriptSign(File.ReadAllText(path).Replace("{port}", Global.Port.ToString()), 0L);
		}
		return result;
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		string script = GetScript();
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", script.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(script, headers);
		return Task.CompletedTask;
	}
}
