using System.Runtime.InteropServices;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.iTunesActions.Properties;

namespace Griffin.PowerMate.iTunesActions;

internal class VolumeDownAction : ComputerActionBase
{
	public override string Name => "Volume Down";

	public override string Description => "Lowers iTunes Volume 5%";

	public override Panel Panel => null;

	public VolumeDownAction(string pluginName)
		: base(pluginName, Resources.volumedown)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		IiTunes iiTunes = new iTunesApp() as IiTunes;
		iiTunes.SoundVolume -= 5;
		Marshal.ReleaseComObject(iiTunes);
		return true;
	}
}
