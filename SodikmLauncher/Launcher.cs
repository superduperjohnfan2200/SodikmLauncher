using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Ionic.Zlib;

namespace SodikmLauncher;

internal class Launcher
{
	public static string GetExecutableName(LaunchType type)
	{
		return type switch
		{
			LaunchType.Player => ClientSettings.Instance.PlayerName, 
			LaunchType.Studio => ClientSettings.Instance.StudioName, 
			_ => throw new Exception("Unknown LaunchType " + Enum.GetName(type)), 
		};
	}

	public static string GetLaunchArgumentsFormat(LaunchType type)
	{
		return type switch
		{
			LaunchType.Player => ClientSettings.Instance.PlayerArgs, 
			LaunchType.Studio => "", 
			_ => throw new Exception("Unknown LaunchType " + Enum.GetName(type)), 
		};
	}

	public static Task Play()
	{
		Launch("http://www.roblox.com/game/join.ashx", LaunchType.Player);
		return Task.CompletedTask;
	}

	public static async Task Host()
	{
		MainWindow mainWindow = Utils.GetMainWindow();
		if (!(mainWindow.MapList.SelectedItem is string mapName))
		{
			MessageBox.Show("Failed to host: no map selected");
			return;
		}
		string mapPath = "./data/maps/" + mapName;
		if (!File.Exists(mapPath))
		{
			MessageBox.Show("Failed to host: failed to find map");
			return;
		}
		byte[] array = await File.ReadAllBytesAsync(mapPath);
		if (mapName.EndsWith(".gz"))
		{
			array = GZipStream.UncompressBuffer(array);
		}
		else if (mapName.EndsWith(".bz2"))
		{
			array = BZip2.Decompress(array);
		}
		await File.WriteAllBytesAsync("./data/web/common/1818.rbxl", array);
		Dictionary<long, byte[]> blobData = null;
		if (!Global.Customisation.Settings.WipePersistence)
		{
			blobData = GameService.Instance.GetBlobData();
		}
		Friends.Clear();
		GameService.Instance.Dispose();
		GameService.Instance = new GameService(mapPath, blobData);
		Launch("http://www.roblox.com/game/gameserver.ashx", LaunchType.Player);
	}

	public static void Launch(string url, LaunchType type, bool useArgs = true)
	{
		string executableName = GetExecutableName(type);
		string arguments = string.Format(GetLaunchArgumentsFormat(type), url);
		string fileName = $"./data/clients/{ClientSettings.Instance.Year}/{Enum.GetName(type)}/{executableName}";
		try
		{
			using Process process = new Process();
			process.StartInfo.FileName = fileName;
			if (useArgs)
			{
				process.StartInfo.Arguments = arguments;
			}
			process.Start();
		}
		catch (Exception value)
		{
			MessageBox.Show($"Failed to launch {ClientSettings.Instance.Year} {Enum.GetName(type)}: {value}", "Sodikm");
		}
	}
}
