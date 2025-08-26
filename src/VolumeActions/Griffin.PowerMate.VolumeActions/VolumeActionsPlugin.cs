using System;
using Griffin.Audio;
using Griffin.PowerMate.App;
using Griffin.PowerMate.VolumeActions.Properties;

namespace Griffin.PowerMate.VolumeActions;

public class VolumeActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "Volume";

	private IOSVolume OsVolume;

	private PowerMateLedSetterThread PowerMateLedThread;

	private VolumeUpAction VolumeUp;

	private VolumeDownAction VolumeDown;

	private VolumeMuteAction VolumeMute;

	public override string Name => PluginName;

	public override string Description => "Controls System Volume";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions => new IComputerAction[3] { VolumeUp, VolumeDown, VolumeMute };

	public VolumeActionsPlugin()
		: base(Resources.volumeUp)
	{
		if (Environment.OSVersion.Version.Major < 6)
		{
			OsVolume = new XPVolume();
		}
		else
		{
			OsVolume = new VistaVolume();
		}
		PowerMateLedThread = new PowerMateLedSetterThread(OsVolume);
		VolumeUp = new VolumeUpAction(PluginName, PowerMateLedThread);
		VolumeDown = new VolumeDownAction(PluginName, PowerMateLedThread);
		VolumeMute = new VolumeMuteAction(PluginName, PowerMateLedThread);
	}

	protected override void OnDisposed(EventArgs e)
	{
		PowerMateLedThread.Dispose();
		if (OsVolume is IDisposable)
		{
			(OsVolume as IDisposable).Dispose();
		}
		base.OnDisposed(e);
	}
}
