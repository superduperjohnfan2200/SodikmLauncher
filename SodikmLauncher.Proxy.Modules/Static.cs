using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy.Modules;

internal class Static : ProxyModule
{
	private static Dictionary<string, string> Responses = new Dictionary<string, string>
	{
		{ "/analytics/measurement.ashx", "" },
		{ "/error/lua.ashx", "" },
		{ "/error/dmp.ashx", "" },
		{ "/error/grid.ashx", "" },
		{ "/game/cdn.ashx", "" },
		{ "/game/joinrate.ashx", "" },
		{ "/game/placevisit.ashx", "" },
		{ "/game/clientpresence.ashx", "" },
		{ "/game/badge/isbadgedisabled.ashx", "0" },
		{ "/game/validate-machine", "{\"success\":true}" },
		{ "/game/logout.aspx", "" },
		{ "/game/keeppingeralive.ashx", "" },
		{ "/game/keepalivepinger.ashx", "" },
		{ "/game/report-stats", "" },
		{ "/game/help.aspx", "I am in your walls" },
		{ "/game/gamepass/gamepasshandler.ashx", "<Value Type=\"boolean\">false</Value>" },
		{ "/analytics/contentprovider.ashx", "" },
		{ "/abusereport/ingamechat.aspx", "You can't run" },
		{ "/uploadmedia/postimage.aspx", "Your images won't save you now... :)" },
		{ "/uploadmedia/uploadvideo.aspx", "Your videos won't save you now... :)" },
		{ "/asset/getscriptstate.ashx", "0 0 0 0" },
		{ "/login/negotiate.ashx", "" },
		{ "/my/places.aspx", "Your studio experience has been brought to you by <CLASSIFIED>" },
		{ "/gametransactions/getpendingtransactions", "[]" },
		{ "/points/get-awardable-points", "{\"points\":\"0\"}" },
		{ "/universes/validate-place-join", "true" },
		{ "/persistence/getv2", "{\"data\":[]}" },
		{ "/persistence/getsortedvalues", "{\"data\":{\"Entries\":[],\"ExclusiveStartKey\":null}}" },
		{ "/persistence/increment", "{\"data\":null}" },
		{ "/persistence/set", "{\"data\":null}" },
		{ "/v1.1/counters/increment", "" },
		{ "/v1.0/multiincrement/", "" },
		{ "/getallowedsecurityversions", "{\"data\":[]}" },
		{ "/getallowedmd5hashes", "{\"data\":[]}" }
	};

	public override bool IsMyUrl(string absolutePath, string host)
	{
		string absolutePath2 = absolutePath;
		return Responses.Any<KeyValuePair<string, string>>((KeyValuePair<string, string> x) => absolutePath2.StartsWith(x.Key) || absolutePath2.EndsWith(x.Key));
	}

	public override Task HandleRequest(SessionEventArgs e)
	{
		string path = e.HttpClient.Request.RequestUri.AbsolutePath.ToLower();
		string value = Responses.FirstOrDefault<KeyValuePair<string, string>>((KeyValuePair<string, string> x) => path.StartsWith(x.Key) || path.EndsWith(x.Key)).Value;
		IEnumerable<HttpHeader> headers = new List<HttpHeader>
		{
			new HttpHeader("Content-Type", "text/plain"),
			new HttpHeader("Content-Length", value.Length.ToString()),
			new HttpHeader("Cache-Control", "no-cache")
		};
		e.Ok(value, headers);
		return Task.CompletedTask;
	}
}
