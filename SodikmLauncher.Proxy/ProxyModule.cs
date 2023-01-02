using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;

namespace SodikmLauncher.Proxy;

public class ProxyModule
{
	public virtual bool IsMyUrl(string absolutePath, string host)
	{
		return false;
	}

	public virtual Task HandleRequest(SessionEventArgs e)
	{
		e.Ok(":O");
		return Task.CompletedTask;
	}
}
