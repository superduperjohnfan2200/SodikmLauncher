using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace SodikmLauncher;

public class App : Application
{
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.7.0")]
	public void InitializeComponent()
	{
		base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
	}

	[STAThread]
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.7.0")]
	public static void Main()
	{
		App app = new App();
		app.InitializeComponent();
		app.Run();
	}
}
