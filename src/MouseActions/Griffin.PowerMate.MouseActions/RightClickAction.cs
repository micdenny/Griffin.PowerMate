using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class RightClickAction : ComputerActionBase
{
	public override string Name => "Right Click";

	public override string Description => "Emulates a Mouse Right Click";

	public override Panel Panel => null;

	public RightClickAction(string pluginName)
		: base(pluginName, Resources.rightClick)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.RightClick();
		return true;
	}
}
