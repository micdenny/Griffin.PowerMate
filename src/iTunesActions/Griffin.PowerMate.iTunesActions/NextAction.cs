using System.Runtime.InteropServices;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.iTunesActions.Properties;

namespace Griffin.PowerMate.iTunesActions;

internal class NextAction : ComputerActionBase
{
	public override string Name => "Next";

	public override string Description => "Skip to Next Track";

	public override Panel Panel => null;

	public NextAction(string pluginName)
		: base(pluginName, Resources.next)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		IiTunes iiTunes = new iTunesApp() as IiTunes;
		iiTunes.NextTrack();
		Marshal.ReleaseComObject(iiTunes);
		return true;
	}
}
