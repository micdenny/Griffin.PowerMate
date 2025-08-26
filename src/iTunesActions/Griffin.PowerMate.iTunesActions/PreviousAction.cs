using System.Runtime.InteropServices;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.iTunesActions.Properties;

namespace Griffin.PowerMate.iTunesActions;

internal class PreviousAction : ComputerActionBase
{
	public override string Name => "Previous";

	public override string Description => "Skip to Beginning or Previous Track";

	public override Panel Panel => null;

	public PreviousAction(string pluginName)
		: base(pluginName, Resources.previous)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		IiTunes iiTunes = new iTunesApp() as IiTunes;
		iiTunes.BackTrack();
		Marshal.ReleaseComObject(iiTunes);
		return true;
	}
}
