using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class BodyColors : ProxyModule
{
	public override bool IsMyUrl(string absolutePath, string host)
	{
		return absolutePath.StartsWith("/asset/bodycolors.ashx");
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		string text = HttpUtility.ParseQueryString(e.HttpClient.Request.RequestUri.Query)["userId"];
		if (string.IsNullOrEmpty(text))
		{
			e.GenericResponse("", HttpStatusCode.InternalServerError);
			return Task.CompletedTask;
		}
		int[] array = text.Split('.').Select(int.Parse).ToArray();
		if (array.Length != 6)
		{
			e.GenericResponse("", HttpStatusCode.InternalServerError);
			return Task.CompletedTask;
		}
		int num = array[0];
		int num2 = array[1];
		int num3 = array[2];
		int num4 = array[3];
		int num5 = array[4];
		int num6 = array[5];
		string text2 = string.Format(LauncherResources.BodyColoursTemplate, num, num2, num4, num3, num5, num6);
		List<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", text2.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(text2, headers);
		return Task.CompletedTask;
	}
}
