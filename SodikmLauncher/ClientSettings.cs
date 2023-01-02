using System;
using System.IO;

namespace SodikmLauncher;

internal class ClientSettings
{
	private FileSystemWatcher Watcher = new FileSystemWatcher();

	public static ClientSettings Instance { get; set; } = new ClientSettings();


	public string Year { get; set; } = "";


	[IniVariable("IsRobloxApp")]
	public bool RobloxApp { get; set; }

	[IniVariable("HasStudio")]
	public bool Studio { get; set; }

	[IniVariable("NewSignatures")]
	public bool NewSignatureFormat { get; set; } = true;


	[IniVariable("PlayerLaunchArgs")]
	public string PlayerArgs { get; set; } = "-j \"{0}\" -a \"http://www.roblox.com/game/negotiate.ashx\" -t \"0\"";


	[IniVariable("PlayerName")]
	public string PlayerName { get; set; } = "RobloxPlayerBeta.exe";


	[IniVariable("StudioName")]
	public string StudioName { get; set; } = "RobloxStudioBeta.exe";


	public void SetupWatcher()
	{
		if (Watcher.EnableRaisingEvents)
		{
			throw new Exception("Watcher is already set up!");
		}
		Watcher.Path = "./data/clients/" + Year;
		Watcher.NotifyFilter = NotifyFilters.LastWrite;
		Watcher.Filter = "Sodikm.ini";
		Watcher.Changed += FileChanged;
		Watcher.EnableRaisingEvents = true;
	}

	private void FileChanged(object source, FileSystemEventArgs e)
	{
		if (SettingsIniParser.TryParse("./data/clients/" + Year + "/Sodikm.ini", Year, out ClientSettings clientSettings))
		{
			Instance = clientSettings;
			Instance.SetupWatcher();
			Dispose();
		}
	}

	public void Dispose()
	{
		Watcher.EnableRaisingEvents = false;
		Watcher.Dispose();
	}
}
