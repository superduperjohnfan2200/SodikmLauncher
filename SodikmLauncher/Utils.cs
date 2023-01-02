using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SodikmLauncher;

internal class Utils
{
	public static readonly List<string> ValidMapExtensions = new List<string> { ".rbxl", ".rbxlx", ".rbxl.gz", ".rbxlx.gz", ".rbxl.bz", ".rbxlx.bz" };

	public static MainWindow GetMainWindow()
	{
		return (MainWindow)Application.Current.MainWindow;
	}

	public static FileSystemWatcher CreateFileSystemWatcher(string path, FileSystemEventHandler handler)
	{
		FileSystemEventHandler handler2 = handler;
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
		fileSystemWatcher.Path = path;
		fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
		fileSystemWatcher.Created += handler2;
		fileSystemWatcher.Deleted += handler2;
		fileSystemWatcher.Changed += handler2;
		fileSystemWatcher.Renamed += delegate
		{
			handler2(null, null);
		};
		fileSystemWatcher.EnableRaisingEvents = true;
		return fileSystemWatcher;
	}
}
