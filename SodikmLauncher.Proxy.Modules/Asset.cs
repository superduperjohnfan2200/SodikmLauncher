using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class Asset : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		if (!absolutePath.EndsWith("/asset"))
		{
			return absolutePath.EndsWith("/asset/");
		}
		return true;
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		string query = e.HttpClient.Request.RequestUri.Query;
		if (!long.TryParse(HttpUtility.ParseQueryString(query)["id"], out var id))
		{
			e.Redirect("https://assetdelivery.roblox.com/v1/asset/" + query);
			return;
		}
		string path = "./data/clients/" + ClientSettings.Instance.Year + "/assets/";
		Directory.CreateDirectory(path);
		string searchPattern = $"{id}.*";
		List<string> list = new List<string>(Directory.GetFiles(path, searchPattern));
		list.AddRange(Directory.GetFiles("./data/web/common/", searchPattern));
		list.AddRange(Directory.GetFiles("./data/web/charapp/", searchPattern));
		string[] directories = Directory.GetDirectories("./data/assetpacks/");
		foreach (string path2 in directories)
		{
			list.AddRange(Directory.GetFiles(path2, searchPattern));
		}
		if (list.Count > 0)
		{
			string text = list[0];
			byte[] array;
			if (text.EndsWith(".lua"))
			{
				Encoding uTF = Encoding.UTF8;
				array = uTF.GetBytes(Crypto.ScriptSign(await File.ReadAllTextAsync(text), id));
			}
			else
			{
				array = await File.ReadAllBytesAsync(text);
			}
			byte[] array2 = array;
			e.Ok(array2, new List<HttpHeader>
			{
				new HttpHeader("Content-Length", array2.LongLength.ToString()),
				new HttpHeader("Cache-Control", "no-cache")
			});
		}
		else
		{
			e.Redirect("https://assetdelivery.roblox.com/v1/asset/" + query);
		}
	}
}
