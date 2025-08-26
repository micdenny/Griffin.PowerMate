using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.ScrollActions.Properties;

namespace Griffin.PowerMate.ScrollActions;

internal class ScrollRightAction : ComputerActionBase
{
	public override string Name => "Scroll Right";

	public override string Description => "Scrolls Right";

	public override Panel Panel => null;

	public ScrollRightAction(string pluginName)
		: base(pluginName, Resources.scrollRight)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.RotateWheelHorizontal(120);
		return true;
	}
}
