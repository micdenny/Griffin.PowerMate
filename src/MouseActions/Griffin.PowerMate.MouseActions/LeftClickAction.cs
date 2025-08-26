using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class LeftClickAction : ComputerActionBase
{
	public override string Name => "Left Click";

	public override string Description => "Emulates a Mouse Left Click";

	public override Panel Panel => null;

	public LeftClickAction(string pluginName)
		: base(pluginName, Resources.leftClick)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.LeftClick();
		return true;
	}
}
