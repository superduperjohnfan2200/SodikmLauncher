using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SodikmLauncher;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class LauncherResources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("SodikmLauncher.LauncherResources", typeof(LauncherResources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string BodyColoursTemplate => ResourceManager.GetString("BodyColoursTemplate", resourceCulture);

	internal static string ClientPath => ResourceManager.GetString("ClientPath", resourceCulture);

	internal static string ClientsPath => ResourceManager.GetString("ClientsPath", resourceCulture);

	internal static string ConfigName => ResourceManager.GetString("ConfigName", resourceCulture);

	internal static string CustomisationSavePath => ResourceManager.GetString("CustomisationSavePath", resourceCulture);

	internal static string GameServerMiddleUrl => ResourceManager.GetString("GameServerMiddleUrl", resourceCulture);

	internal static string GameServerUrl => ResourceManager.GetString("GameServerUrl", resourceCulture);

	internal static string JoinUrl => ResourceManager.GetString("JoinUrl", resourceCulture);

	internal static string MapsPath => ResourceManager.GetString("MapsPath", resourceCulture);

	internal LauncherResources()
	{
	}
}
