using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.ScrollActions.Properties;

namespace Griffin.PowerMate.ScrollActions;

internal class ScrollLeftAction : ComputerActionBase
{
	public override string Name => "Scroll Left";

	public override string Description => "Scrolls Left";

	public override Panel Panel => null;

	public ScrollLeftAction(string pluginName)
		: base(pluginName, Resources.scrollLeft)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.RotateWheelHorizontal(-120);
		return true;
	}
}
