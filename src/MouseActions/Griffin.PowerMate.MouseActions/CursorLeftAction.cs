using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

internal class CursorLeftAction : ComputerActionBase
{
	public override string Name => "Cursor Left";

	public override string Description => "Moves the Mouse Cursor to the Left";

	public override Panel Panel => null;

	public CursorLeftAction(string pluginName)
		: base(pluginName, Resources.cursorLeft)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Mouse.MoveCursor(-10, 0);
		return true;
	}
}
