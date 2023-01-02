using System;

namespace SodikmLauncher;

internal class IniVariableAttribute : Attribute
{
	public readonly string Name;

	public IniVariableAttribute(string name)
	{
		Name = name;
	}
}
