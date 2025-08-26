using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class CursorUpAction : ComputerActionBase
{
	public override string Name => "Cursor Up";

	public override string Description => "Moves the Mouse Cursor Up";

	public override Panel Panel => null;

	public CursorUpAction(string pluginName)
		: base(pluginName, Resources.cursorUp)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.MoveCursor(0, -10);
		return true;
	}
}
