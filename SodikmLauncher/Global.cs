using System.Net;
using System.Net.Http;

namespace SodikmLauncher;

internal class Global
{
	public static readonly HttpClient HttpClient = new HttpClient(new HttpClientHandler
	{
		AutomaticDecompression = DecompressionMethods.All
	});

	public static Customisation Customisation = null;

	public static string IP = "localhost";

	public static int Port = 53640;
}
