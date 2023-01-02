using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Win32;
using SodikmLauncher.Proxy;

namespace SodikmLauncher;

public partial class MainWindow : Window, IComponentConnector, IStyleConnector
{
	private FileSystemWatcher? MapWatcher;

	private const string F = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

	public MainWindow()
	{
		InitializeComponent();
		Global.Customisation = new Customisation(this);
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Thread thread = new Thread((ParameterizedThreadStart)delegate
		{
			SodikmLauncher.Proxy.Proxy.Start();
		});
		thread.IsBackground = true;
		thread.Start();
		Directory.CreateDirectory(LauncherResources.ClientsPath);
		Directory.CreateDirectory(LauncherResources.MapsPath);
		Directory.CreateDirectory("./data/assetpacks/");
		CreateClientList();
		CreateMapList();
		MapWatcher = Utils.CreateFileSystemWatcher(LauncherResources.MapsPath, delegate
		{
			CreateMapList();
		});
		ColourSelection.ItemsSource = BrickColour.RGB;
		base.Title = $"{base.Title} [{Assembly.GetExecutingAssembly().GetName().Version}]";
		UsernameBox.Text = Global.Customisation.Settings.Username;
		IDBox.Text = Global.Customisation.Settings.Id.ToString();
		WipeGamePersistenceCheckBox.IsChecked = Global.Customisation.Settings.WipePersistence;
		ProxyRequestsOnlyFromClient.IsChecked = Global.Customisation.Settings.ProxyOnlyRobloxUserAgent;
		Global.Customisation.Load();
	}

	private void CreateClientList()
	{
		ClientList.Items.Clear();
		string[] directories = Directory.GetDirectories(LauncherResources.ClientsPath);
		foreach (string text in directories)
		{
			if (File.Exists(text + "/" + LauncherResources.ConfigName))
			{
				ClientList.Items.Add(text.Substring(text.LastIndexOf("\\") + 1));
			}
		}
		ClientList.SelectedIndex = 0;
	}

	private void CreateMapList()
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			MapList.Items.Clear();
			string[] files = Directory.GetFiles(LauncherResources.MapsPath);
			foreach (string map in files)
			{
				if (Utils.ValidMapExtensions.Any((string x) => map.EndsWith(x)))
				{
					MapList.Items.Add(map.Substring(map.LastIndexOf("\\") + 1));
				}
			}
			MapList.SelectedIndex = 0;
		});
	}

	private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		string text = ClientList.SelectedItem.ToString();
		if (text != null)
		{
			if (!SettingsIniParser.TryParse("./data/clients/" + text + "/Sodikm.ini", text, out ClientSettings clientSettings))
			{
				MessageBox.Show("Failed to parse \"" + text + "\"'s Sodikm configuration file!", "Sodikm");
				return;
			}
			ClientSettings.Instance.Dispose();
			ClientSettings.Instance = clientSettings;
			ClientSettings.Instance.SetupWatcher();
		}
	}

	private async void JoinButton_Click(object sender, RoutedEventArgs e)
	{
		await Launcher.Play();
	}

	private async void HostButton_Click(object sender, RoutedEventArgs e)
	{
		await Launcher.Host();
	}

	private void StudioButton_Click(object sender, RoutedEventArgs e)
	{
		if (!ClientSettings.Instance.Studio && !ClientSettings.Instance.RobloxApp)
		{
			MessageBox.Show("Selected year has no studio available", "Sodikm");
		}
		else
		{
			Launcher.Launch("", (!ClientSettings.Instance.RobloxApp) ? LaunchType.Studio : LaunchType.Player, useArgs: false);
		}
	}

	private void ColourSelectionItem_MouseDown(object sender, MouseButtonEventArgs e)
	{
		Label label = (Label)sender;
		int key = BrickColour.RGB.ToList().Find((KeyValuePair<int, Brush> x) => x.Value == label.Background).Key;
		Global.Customisation.SelectedColourID = key;
	}

	private void ColourReset_Click(object sender, RoutedEventArgs e)
	{
		Global.Customisation.SetLimbsColourToID(24, 23, 24, 24, 119, 119);
	}

	private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (Global.Customisation != null)
		{
			Global.Customisation.Settings.Username = UsernameBox.Text;
		}
	}

	private void IDBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (Global.Customisation != null)
		{
			if (int.TryParse(IDBox.Text, out var result))
			{
				Global.Customisation.Settings.Id = result;
			}
			else
			{
				IDBox.Text = Global.Customisation.Settings.Id.ToString();
			}
		}
	}

	private void WipeGamePersistenceCheckBox_Click(object sender, RoutedEventArgs e)
	{
		Global.Customisation.Settings.WipePersistence = WipeGamePersistenceCheckBox.IsChecked ?? true;
	}

	private void ImportSOBLButton_Click(object sender, RoutedEventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Title = "Load Sodikm Blob",
			Filter = "Sodikm Blob Save|*.sobl"
		};
		if (openFileDialog.ShowDialog() != true)
		{
			return;
		}
		using StreamReader streamReader = new StreamReader((FileStream)openFileDialog.OpenFile());
		using MemoryStream memoryStream = new MemoryStream();
		streamReader.BaseStream.CopyTo(memoryStream);
		MessageBox.Show(GameService.Instance.ImportSOBL(memoryStream.ToArray()) ? "Imported save file" : "Failed to load save file", "Sodikm");
	}

	private void ExportSOBLButton_Click(object sender, RoutedEventArgs e)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			Title = "Save Sodikm Blob",
			Filter = "Sodikm Blob Save|*.sobl"
		};
		if (saveFileDialog.ShowDialog() == true)
		{
			using (FileStream fileStream = saveFileDialog.OpenFile() as FileStream)
			{
				fileStream?.Write(GameService.Instance.ExportSOBL());
			}
			MessageBox.Show("Exported save file", "Sodikm");
		}
	}

	private void IPBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		Global.IP = IPBox.Text;
	}

	private void PortBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (int.TryParse(PortBox.Text, out var result))
		{
			Global.Port = result;
		}
		else
		{
			PortBox.Text = Global.Port.ToString();
		}
	}

	private void Window_Closed(object sender, EventArgs e)
	{
		Global.Customisation.SaveSettings();
		try
		{
			SodikmLauncher.Proxy.Proxy.Stop();
		}
		catch
		{
		}
	}

	private void ProxyRequestsFromClient_Click(object sender, RoutedEventArgs e)
	{
		Global.Customisation.Settings.ProxyOnlyRobloxUserAgent = ProxyRequestsOnlyFromClient.IsChecked.GetValueOrDefault();
	}
}
