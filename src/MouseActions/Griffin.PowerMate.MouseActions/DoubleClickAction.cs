using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class DoubleClickAction : ComputerActionBase
{
	public override string Name => "Double Click";

	public override string Description => "Emulates a Mouse Left Double-Click";

	public override Panel Panel => null;

	public DoubleClickAction(string pluginName)
		: base(pluginName, Resources.doubleClick)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.LeftDoubleClick();
		return true;
	}
}
