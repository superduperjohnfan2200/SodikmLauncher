using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;

namespace SodikmLauncher.Proxy;

internal class Proxy
{
	private static List<ProxyModule> Modules = new List<ProxyModule>();

	private static ProxyServer ProxyServer = new ProxyServer();

	private static void PopulateModulesList()
	{
		foreach (Type item in from assembly in AppDomain.CurrentDomain.GetAssemblies()
			from type in assembly.GetTypes()
			where type.IsSubclassOf(typeof(ProxyModule))
			select type)
		{
			Modules.Add((ProxyModule)Activator.CreateInstance(item));
		}
	}

	private static bool IsValid(HttpWebClient client)
	{
		string host = client.Request.RequestUri.Host;
		HeaderCollection headers = client.Request.Headers;
		if (((host.StartsWith("www.") || host.StartsWith("web.") || host.StartsWith("assetgame.") || host.StartsWith("wiki.") || host.EndsWith("api.roblox.com") || host.StartsWith("roblox.com")) && host.EndsWith("roblox.com")) || host.EndsWith("robloxlabs.com"))
		{
			if (Global.Customisation.Settings.ProxyOnlyRobloxUserAgent)
			{
				List<HttpHeader>? headers2 = headers.GetHeaders("User-Agent");
				if (headers2 == null)
				{
					return false;
				}
				return headers2.FirstOrDefault()?.Value.ToLowerInvariant().Contains("roblox") == true;
			}
			return true;
		}
		return false;
	}

	private static Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
	{
		if (!IsValid(e.HttpClient))
		{
			e.DecryptSsl = false;
		}
		return Task.CompletedTask;
	}

	private static async Task OnRequest(object sender, SessionEventArgs e)
	{
		string host = e.HttpClient.Request.RequestUri.Host;
		if (!IsValid(e.HttpClient))
		{
			return;
		}
		ProxyModule[] array = Modules.ToArray();
		foreach (ProxyModule proxyModule in array)
		{
			if (proxyModule.IsMyUrl(e.HttpClient.Request.RequestUri.AbsolutePath.ToLowerInvariant(), host))
			{
				try
				{
					await proxyModule.HandleRequest(e);
					return;
				}
				catch (Exception)
				{
					e.GenericResponse("", HttpStatusCode.InternalServerError);
					return;
				}
			}
		}
		e.GenericResponse("", HttpStatusCode.NotFound);
	}

	public static void Start()
	{
		try
		{
			PopulateModulesList();
			ProxyServer.BeforeRequest += OnRequest;
			ExplicitProxyEndPoint explicitProxyEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000);
			explicitProxyEndPoint.BeforeTunnelConnectRequest += OnBeforeTunnelConnectRequest;
			ProxyServer.AddEndPoint(explicitProxyEndPoint);
			ProxyServer.Start();
			foreach (ProxyEndPoint proxyEndPoint in ProxyServer.ProxyEndPoints)
			{
				_ = proxyEndPoint;
			}
			ProxyServer.SetAsSystemHttpProxy(explicitProxyEndPoint);
			ProxyServer.SetAsSystemHttpsProxy(explicitProxyEndPoint);
		}
		catch (Exception value)
		{
			MessageBox.Show($"Failed to initialise proxy: {value}", "Sodikm");
		}
	}

	public static void Stop()
	{
		ProxyServer.BeforeRequest -= OnRequest;
		ProxyServer.Stop();
	}
}
