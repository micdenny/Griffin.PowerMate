using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class CursorRightAction : ComputerActionBase
{
	public override string Name => "Cursor Right";

	public override string Description => "Moves the Mouse Cursor to the Right";

	public override Panel Panel => null;

	public CursorRightAction(string pluginName)
		: base(pluginName, Resources.cursorRight)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.MoveCursor(10, 0);
		return true;
	}
}
