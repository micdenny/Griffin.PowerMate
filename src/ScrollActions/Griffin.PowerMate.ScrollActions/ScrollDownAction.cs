using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.ScrollActions.Properties;

namespace Griffin.PowerMate.ScrollActions;

internal class ScrollDownAction : ComputerActionBase
{
	public override string Name => "Scroll Down";

	public override string Description => "Scrolls Down";

	public override Panel Panel => null;

	public ScrollDownAction(string pluginName)
		: base(pluginName, Resources.scrollDown)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.RotateWheel(-120);
		return true;
	}
}
