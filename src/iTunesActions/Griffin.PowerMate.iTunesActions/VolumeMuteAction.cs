using System.Runtime.InteropServices;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.iTunesActions.Properties;

namespace Griffin.PowerMate.iTunesActions;

internal class VolumeMuteAction : ComputerActionBase
{
	public override string Name => "Mute";

	public override string Description => "Mutes iTunes Volume";

	public override Panel Panel => null;

	public VolumeMuteAction(string pluginName)
		: base(pluginName, Resources.volumemute)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		IiTunes iiTunes = new iTunesApp() as IiTunes;
		iiTunes.Mute = !iiTunes.Mute;
		Marshal.ReleaseComObject(iiTunes);
		return true;
	}
}
