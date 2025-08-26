using System;
using System.Drawing;
using Griffin.PowerMate.App;
using Microsoft.Win32;

namespace Griffin.PowerMate.iTunesActions;

public class iTunesActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "iTunes";

	private PlayPauseAction PlayPause = new PlayPauseAction(PluginName);

	private PreviousAction Previous = new PreviousAction(PluginName);

	private NextAction Next = new NextAction(PluginName);

	private VolumeUpAction VolumeUp = new VolumeUpAction(PluginName);

	private VolumeDownAction VolumeDown = new VolumeDownAction(PluginName);

	private VolumeMuteAction VolumeMute = new VolumeMuteAction(PluginName);

	public override string Name => PluginName;

	public override string Description => "iTunes Specific Controls";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions => new IComputerAction[6] { PlayPause, Previous, Next, VolumeUp, VolumeDown, VolumeMute };

	internal static Icon iTunesIcon
	{
		get
		{
			bool flag = false;
			Icon icon = null;
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Apple Computer, Inc.\\iTunes");
			if (registryKey == null && IntPtr.Size == 8)
			{
				registryKey = Registry.LocalMachine.OpenSubKey("Software\\WOW6432Node\\Apple Computer, Inc.\\iTunes");
				flag = true;
			}
			if (registryKey != null)
			{
				string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + (string)registryKey.GetValue("ProgramFolder") + "iTunes.exe";
				if (flag && !string.IsNullOrEmpty(text))
				{
					int num = text.IndexOf(":\\Program Files") + 15;
					if (num >= 0)
					{
						text = text.Insert(num, " (x86)");
					}
				}
				registryKey.Close();
				icon = IconHelper.GetIconFromFile(text, 0, IconSize.Large);
			}
			if (icon == null)
			{
				icon = IconHelper.GetIconFromFile(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\shell32.dll", 188, IconSize.Large);
			}
			return icon;
		}
	}

	public iTunesActionsPlugin()
		: base(iTunesIcon)
	{
	}
}
