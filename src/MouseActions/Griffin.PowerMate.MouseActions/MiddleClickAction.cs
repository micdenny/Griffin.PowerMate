using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class MiddleClickAction : ComputerActionBase
{
	public override string Name => "Middle Click";

	public override string Description => "Emulates a Mouse Middle Click";

	public override Panel Panel => null;

	public MiddleClickAction(string pluginName)
		: base(pluginName, Resources.middleClick)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.MiddleClick();
		return true;
	}
}
