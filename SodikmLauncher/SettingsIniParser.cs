using System;
using System.Reflection;
using IniParser;
using IniParser.Model;

namespace SodikmLauncher;

internal class SettingsIniParser
{
	private static FileIniDataParser IniParser = new FileIniDataParser();

	public static ClientSettings Parse(string fileName, string year)
	{
		IniData iniData = IniParser.ReadFile(fileName);
		ClientSettings clientSettings = new ClientSettings();
		PropertyInfo[] properties = clientSettings.GetType().GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			IniVariableAttribute iniVariableAttribute = (IniVariableAttribute)propertyInfo.GetCustomAttribute(typeof(IniVariableAttribute), inherit: false);
			if (iniVariableAttribute != null && iniData.TryGetKey(iniVariableAttribute.Name, out var value))
			{
				propertyInfo.SetValue(clientSettings, Convert.ChangeType(value, propertyInfo.PropertyType));
			}
		}
		clientSettings.Year = year;
		return clientSettings;
	}

	public static bool TryParse(string fileName, string year, out ClientSettings clientSettings)
	{
		try
		{
			clientSettings = Parse(fileName, year);
		}
		catch (Exception)
		{
			clientSettings = null;
			return false;
		}
		return true;
	}
}
