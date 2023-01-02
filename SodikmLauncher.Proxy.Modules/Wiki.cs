using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class Wiki : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		if (absolutePath.StartsWith("/images/"))
		{
			return host.StartsWith("wiki.");
		}
		return false;
	}

	public override async Task HandleRequest(SessionEventArgs e)
	{
		string absolutePath = e.HttpClient.Request.RequestUri.AbsolutePath;
		int num = absolutePath.LastIndexOf('/');
		if (num == -1)
		{
			e.GenericResponse("", HttpStatusCode.NotFound);
			return;
		}
		string text = absolutePath.Substring(num + 1);
		string path = "./data/web/wiki/" + text;
		if (!File.Exists(path))
		{
			e.GenericResponse("", HttpStatusCode.NotFound);
			return;
		}
		byte[] array = await File.ReadAllBytesAsync(path);
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Length", array.LongLength.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(array, headers);
	}
}
