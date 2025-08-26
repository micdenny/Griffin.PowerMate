using System.Runtime.InteropServices;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.iTunesActions.Properties;

namespace Griffin.PowerMate.iTunesActions;

internal class PlayPauseAction : ComputerActionBase
{
	public override string Name => "Play/Pause";

	public override string Description => "Toggles the Play/Pause State";

	public override Panel Panel => null;

	public PlayPauseAction(string pluginName)
		: base(pluginName, Resources.play)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		IiTunes iiTunes = new iTunesApp() as IiTunes;
		iiTunes.PlayPause();
		Marshal.ReleaseComObject(iiTunes);
		return true;
	}
}
