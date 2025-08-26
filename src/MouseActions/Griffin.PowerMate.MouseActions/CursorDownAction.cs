using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class CursorDownAction : ComputerActionBase
{
	public override string Name => "Cursor Down";

	public override string Description => "Moves the Mouse Cursor Down";

	public override Panel Panel => null;

	public CursorDownAction(string pluginName)
		: base(pluginName, Resources.cursorDown)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.MoveCursor(0, 10);
		return true;
	}
}
