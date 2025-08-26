using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.ScrollActions.Properties;

namespace Griffin.PowerMate.ScrollActions;

internal class ScrollUpAction : ComputerActionBase
{
	public override string Name => "Scroll Up";

	public override string Description => "Scrolls Up";

	public override Panel Panel => null;

	public ScrollUpAction(string pluginName)
		: base(pluginName, Resources.scrollUp)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.RotateWheel(120);
		return true;
	}
}
