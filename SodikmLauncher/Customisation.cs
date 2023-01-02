using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SodikmLauncher;

internal class Customisation
{
	public class SaveFile
	{
		public int Id { get; set; } = Faker.GenerateId();


		public string Username { get; set; } = Faker.GenerateUsername();


		public bool WipePersistence { get; set; } = true;


		public bool ProxyOnlyRobloxUserAgent { get; set; }

		public int Head { get; set; } = 24;


		public int Torso { get; set; } = 23;


		public int LeftArm { get; set; } = 24;


		public int RightArm { get; set; } = 24;


		public int LeftLeg { get; set; } = 119;


		public int RightLeg { get; set; } = 119;


		public long TShirt { get; set; }

		public long Shirt { get; set; }

		public long Pants { get; set; }

		public long Face { get; set; }

		public long Hat1 { get; set; }

		public long Hat2 { get; set; }

		public long Hat3 { get; set; }

		public long TorsoPackage { get; set; }

		public long LeftArmPackage { get; set; }

		public long RightArmPackage { get; set; }

		public long LeftLegPackage { get; set; }

		public long RightLegPackage { get; set; }

		public long HeadPackage { get; set; }
	}

	private readonly MainWindow Window;

	private readonly List<Grid> Grids = new List<Grid>();

	private Dictionary<int, List<AssetData>> Assets = new Dictionary<int, List<AssetData>>();

	public SaveFile Settings;

	public int SelectedColourID = 1;

	private List<AssetInformation> CachedAssets = new List<AssetInformation>();

	public Customisation(MainWindow window)
	{
		Window = window;
		ParseAssets();
		LoadSettings(out Settings);
	}

	public void Load()
	{
		CreateColourGrid("Color", Window.ColourGrid, Window.ColourButton);
		CreateThreeGrid("Hats", 1, Window.HatGrid, Window.HatButton);
		CreateOneGrid("Shirt", 2, Window.ShirtGrid, Window.ShirtButton);
		CreateOneGrid("Pants", 3, Window.PantsGrid, Window.PantsButton);
		CreateOneGrid("T-Shirt", 4, Window.TShirtGrid, Window.TShirtButton);
		CreateOneGrid("Face", 5, Window.FaceGrid, Window.FaceButton);
		CreateOneGrid("Torso Package", 6, Window.TorsoGrid, Window.TorsoButton);
		CreateOneGrid("Left Arm Package", 7, Window.LeftArmGrid, Window.LeftArmButton);
		CreateOneGrid("Right Arm Package", 8, Window.RightArmGrid, Window.RightArmButton);
		CreateOneGrid("Left Leg Package", 9, Window.LeftLegGrid, Window.LeftLegButton);
		CreateOneGrid("Right Leg Package", 10, Window.RightLegGrid, Window.RightLegButton);
		CreateOneGrid("Head Package", 11, Window.HeadGrid, Window.HeadButton);
	}

	private void LoadSettings(out SaveFile settings)
	{
		if (!File.Exists(LauncherResources.CustomisationSavePath))
		{
			settings = new SaveFile();
			SaveSettings();
			return;
		}
		try
		{
			SaveFile saveFile = JsonSerializer.Deserialize<SaveFile>(File.ReadAllText(LauncherResources.CustomisationSavePath));
			if (saveFile == null)
			{
				throw new Exception("Deserialised json is null");
			}
			settings = saveFile;
		}
		catch (Exception)
		{
			settings = new SaveFile();
			SaveSettings();
		}
	}

	public void SaveSettings()
	{
		string contents = JsonSerializer.Serialize(Settings);
		File.WriteAllText(LauncherResources.CustomisationSavePath, contents);
	}

	private void HandleButtonClick(string type, Grid grid)
	{
		Grids.ForEach(delegate(Grid x)
		{
			x.Visibility = Visibility.Hidden;
		});
		grid.Visibility = Visibility.Visible;
	}

	private void ImageOrFallbackText(Button button, Image image, string imagePath, string? fallback)
	{
		string fullPath = Path.GetFullPath(imagePath);
		if (!File.Exists(fullPath))
		{
			button.Content = fallback;
			image.Source = null;
		}
		else
		{
			BitmapImage source = new BitmapImage(new Uri(fullPath));
			image.Source = source;
			button.Content = image;
		}
	}

	private void CreateOneGrid(string name, int category, Grid grid, Button showButton)
	{
		string name2 = name;
		Grid grid2 = grid;
		ListBox itemList = new ListBox
		{
			Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
			Foreground = new SolidColorBrush(Colors.White),
			BorderBrush = new SolidColorBrush(Colors.Black),
			HorizontalAlignment = HorizontalAlignment.Left,
			Width = 180.0,
			Margin = new Thickness(10.0, 10.0, 0.0, 10.0)
		};
		DataTemplate dataTemplate = new DataTemplate();
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(StackPanel));
		FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(TextBlock));
		frameworkElementFactory2.SetBinding(TextBlock.TextProperty, new Binding("Name"));
		frameworkElementFactory.AppendChild(frameworkElementFactory2);
		dataTemplate.VisualTree = frameworkElementFactory;
		itemList.ItemTemplate = dataTemplate;
		Button imageButton = new Button
		{
			Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
			Foreground = new SolidColorBrush(Colors.White),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Center,
			Width = 140.0,
			Height = 140.0,
			Content = "No item selected",
			Margin = new Thickness(224.0, 0.0, 0.0, 0.0)
		};
		Image image = new Image();
		showButton.Click += delegate
		{
			HandleButtonClick(name2, grid2);
		};
		Grids.Add(grid2);
		PropertyInfo property = Settings.GetType().GetProperty(name2.Replace("-", "").Replace(" ", ""));
		if (Assets.ContainsKey(category))
		{
			List<AssetData> list = Assets[category];
			itemList.ItemsSource = list;
			itemList.SelectionChanged += delegate
			{
				if (!(itemList.SelectedItem is AssetData assetData2))
				{
					image.Source = null;
					imageButton.Content = "No item selected";
					property.SetValue(Settings, 0);
				}
				else
				{
					property.SetValue(Settings, assetData2.Id);
					imageButton.Content = assetData2.Name;
					ImageOrFallbackText(imageButton, image, $"./data/web/thumbs/{assetData2.Id}.png", assetData2.Name);
				}
			};
			imageButton.Click += delegate
			{
				itemList.SelectedItem = null;
			};
			long settingsSelected = (long)property.GetValue(Settings);
			AssetData assetData = list.Find((AssetData x) => x.Id == settingsSelected);
			if (assetData != null)
			{
				itemList.SelectedItem = assetData;
			}
			else
			{
				property.SetValue(Settings, 0);
			}
		}
		else
		{
			imageButton.Content = $"Category {category} empty";
		}
		grid2.Children.Add(itemList);
		grid2.Children.Add(imageButton);
	}

	private bool IsHatAlreadyEquipped(long id)
	{
		if (id != 0L)
		{
			if (Settings.Hat1 != id && Settings.Hat2 != id)
			{
				return Settings.Hat3 == id;
			}
			return true;
		}
		return false;
	}

	private void CreateThreeGrid(string name, int category, Grid grid, Button showButton)
	{
		string name2 = name;
		Grid grid2 = grid;
		ListBox itemList = new ListBox
		{
			Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
			Foreground = new SolidColorBrush(Colors.White),
			BorderBrush = new SolidColorBrush(Colors.Black),
			HorizontalAlignment = HorizontalAlignment.Left,
			Width = 180.0,
			Margin = new Thickness(10.0, 10.0, 0.0, 10.0)
		};
		DataTemplate dataTemplate = new DataTemplate();
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(StackPanel));
		FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(TextBlock));
		frameworkElementFactory2.SetBinding(TextBlock.TextProperty, new Binding("Name"));
		frameworkElementFactory.AppendChild(frameworkElementFactory2);
		dataTemplate.VisualTree = frameworkElementFactory;
		itemList.ItemTemplate = dataTemplate;
		Button button2 = new Button
		{
			Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
			Foreground = new SolidColorBrush(Colors.White),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
			Width = 70.0,
			Height = 70.0,
			Content = "No item selected",
			Margin = new Thickness(217.0, 16.0, 0.0, 0.0)
		};
		Button button3 = new Button
		{
			Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
			Foreground = new SolidColorBrush(Colors.White),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Center,
			Width = 70.0,
			Height = 70.0,
			Content = "No item selected",
			Margin = new Thickness(290.0, 0.0, 0.0, 0.0)
		};
		Button button4 = new Button
		{
			Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
			Foreground = new SolidColorBrush(Colors.White),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
			Width = 70.0,
			Height = 70.0,
			Content = "No item selected",
			Margin = new Thickness(217.0, 166.0, 0.0, 0.0)
		};
		showButton.Click += delegate
		{
			HandleButtonClick(name2, grid2);
		};
		Grids.Add(grid2);
		Dictionary<Button, Tuple<string, Image>> buttons = new Dictionary<Button, Tuple<string, Image>>
		{
			{
				button2,
				new Tuple<string, Image>("Hat1", new Image())
			},
			{
				button3,
				new Tuple<string, Image>("Hat2", new Image())
			},
			{
				button4,
				new Tuple<string, Image>("Hat3", new Image())
			}
		};
		grid2.Children.Add(itemList);
		buttons.ToList().ForEach(delegate(KeyValuePair<Button, Tuple<string, Image>> x)
		{
			grid2.Children.Add(x.Key);
		});
		if (!Assets.ContainsKey(category))
		{
			button2.Content = $"Category {category} empty";
			button3.Content = "Matt was here";
			button4.Content = "YOLO";
			return;
		}
		List<AssetData> list = Assets[category];
		itemList.ItemsSource = list;
		Button selectedButton = buttons.First().Key;
		foreach (KeyValuePair<Button, Tuple<string, Image>> item4 in buttons)
		{
			Button button = item4.Key;
			string item = item4.Value.Item1;
			Image image = item4.Value.Item2;
			PropertyInfo property = Settings.GetType().GetProperty(item);
			button.PreviewMouseDown += delegate(object sender, MouseButtonEventArgs e)
			{
				if (e.ChangedButton == MouseButton.Left)
				{
					selectedButton = button;
				}
				else if (e.ChangedButton == MouseButton.Right)
				{
					property.SetValue(Settings, 0);
					image.Source = null;
					button.Content = "No item selected";
				}
			};
			long selectedHatId = (long)property.GetValue(Settings);
			if (selectedHatId > 0)
			{
				AssetData assetData = list.Find((AssetData x) => x.Id == selectedHatId);
				if (assetData == null)
				{
					property.SetValue(Settings, 0);
					continue;
				}
				ImageOrFallbackText(button, image, $"./data/web/thumbs/{assetData.Id}.png", assetData.Name);
			}
		}
		itemList.SelectionChanged += delegate
		{
			if (itemList.SelectedItem is AssetData assetData2 && !IsHatAlreadyEquipped(assetData2.Id))
			{
				Tuple<string, Image> tuple = buttons[selectedButton];
				string item2 = tuple.Item1;
				Image item3 = tuple.Item2;
				Settings.GetType().GetProperty(item2).SetValue(Settings, assetData2.Id);
				selectedButton.Content = assetData2.Name;
				ImageOrFallbackText(selectedButton, item3, $"./data/web/thumbs/{assetData2.Id}.png", assetData2.Name);
			}
		};
	}

	public void SetLimbsColourToID(int head, int torso, int leftArm, int rightArm, int leftLeg, int rightLeg)
	{
		SetLimbColourToID(Window.ColourHead, head);
		Settings.Head = head;
		SetLimbColourToID(Window.ColourTorso, torso);
		Settings.Torso = torso;
		SetLimbColourToID(Window.ColourLeftArm, leftArm);
		Settings.LeftArm = leftArm;
		SetLimbColourToID(Window.ColourRightArm, rightArm);
		Settings.RightArm = rightArm;
		SetLimbColourToID(Window.ColourLeftLeg, leftLeg);
		Settings.LeftLeg = leftLeg;
		SetLimbColourToID(Window.ColourRightLeg, rightLeg);
		Settings.RightLeg = rightLeg;
	}

	public void SetLimbColourToID(Button limb, int id)
	{
		limb.Background = BrickColour.RGB[id];
	}

	private void CreateColourGrid(string name, Grid grid, Button showButton)
	{
		string name2 = name;
		Grid grid2 = grid;
		showButton.Click += delegate
		{
			HandleButtonClick(name2, grid2);
		};
		Grids.Add(grid2);
		foreach (KeyValuePair<Button, string> entry in new Dictionary<Button, string>
		{
			{ Window.ColourHead, "Head" },
			{ Window.ColourTorso, "Torso" },
			{ Window.ColourLeftArm, "LeftArm" },
			{ Window.ColourRightArm, "RightArm" },
			{ Window.ColourLeftLeg, "LeftLeg" },
			{ Window.ColourRightLeg, "RightLeg" }
		})
		{
			PropertyInfo property = Settings.GetType().GetProperty(entry.Value);
			entry.Key.Click += delegate
			{
				property.SetValue(Settings, SelectedColourID);
				SetLimbColourToID(entry.Key, SelectedColourID);
			};
			SetLimbColourToID(entry.Key, (int)property.GetValue(Settings));
		}
	}

	private void ParseAssets()
	{
		Directory.CreateDirectory("./data/web/assetmeta/");
		string[] files = Directory.GetFiles("./data/web/assetmeta/", "*.info.json");
		foreach (string path in files)
		{
			AssetData assetData = JsonSerializer.Deserialize<AssetData>(File.ReadAllText(path));
			if (assetData == null || assetData.Type < 1 || string.IsNullOrEmpty(assetData.Name))
			{
				continue;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			if (long.TryParse(fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - 5), out var result))
			{
				assetData.Id = result;
				if (!Assets.ContainsKey(assetData.Type))
				{
					Assets[assetData.Type] = new List<AssetData>();
				}
				Assets[assetData.Type].Add(assetData);
			}
		}
		foreach (KeyValuePair<int, List<AssetData>> asset in Assets)
		{
			Assets[asset.Key] = asset.Value.OrderBy((AssetData x) => x.Name).ToList();
		}
	}

	private long[] GetUncachedAssets(long[] ids)
	{
		List<long> list = new List<long>();
		for (int i = 0; i < ids.Length; i++)
		{
			long id = ids[i];
			if (!CachedAssets.Any((AssetInformation x) => x.requestId == id.ToString()) && !Assets.Any<KeyValuePair<int, List<AssetData>>>((KeyValuePair<int, List<AssetData>> x) => x.Value.Any((AssetData y) => y.Id == id)) && id > 0)
			{
				list.Add(id);
			}
		}
		return list.ToArray();
	}

	public async Task<long[]> FilterWhitelisted(long[] ids)
	{
		long[] uniqueIds = ids.ToList().Distinct().ToArray();
		long[] uncachedAssets = GetUncachedAssets(uniqueIds);
		if (uncachedAssets.LongLength > 0)
		{
			AssetInformation[] collection = await Asset.BatchRequest(uncachedAssets);
			CachedAssets.AddRange(collection);
		}
		List<long> list = new List<long>();
		long[] array = uniqueIds;
		for (int i = 0; i < array.Length; i++)
		{
			long id = array[i];
			int? num = CachedAssets.Find((AssetInformation x) => x.requestId == id.ToString())?.assetTypeId;
			if (Assets.Any<KeyValuePair<int, List<AssetData>>>((KeyValuePair<int, List<AssetData>> x) => x.Value.Any((AssetData y) => y.Id == id)) || num == 2 || num == 11 || num == 12)
			{
				list.Add(id);
			}
		}
		return list.ToArray();
	}

	public string ConstructCharappUrl()
	{
		string str = $"{Settings.Head}.{Settings.LeftArm}.{Settings.RightArm}.{Settings.LeftLeg}.{Settings.RightLeg}.{Settings.Torso},{Settings.TShirt},{Settings.Shirt},{Settings.Pants},{Settings.Hat1},{Settings.Hat2},{Settings.Hat3},{Settings.Face},{Settings.TorsoPackage},{Settings.LeftArmPackage},{Settings.RightArmPackage},{Settings.LeftLegPackage},{Settings.RightLegPackage},{Settings.HeadPackage}";
		return "http://www.roblox.com/Asset/CharacterFetch.ashx?userId=" + HttpUtility.UrlEncode(str);
	}
}
