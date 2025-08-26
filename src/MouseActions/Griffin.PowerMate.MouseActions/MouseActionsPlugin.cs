using Griffin.PowerMate.App;
using Griffin.PowerMate.MouseActions.Properties;

namespace Griffin.PowerMate.MouseActions;

public class MouseActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "Mouse";

	private LeftClickAction LeftClick = new LeftClickAction(PluginName);

	private DoubleClickAction DoubleClick = new DoubleClickAction(PluginName);

	private MiddleClickAction MiddleClick = new MiddleClickAction(PluginName);

	private RightClickAction RightClick = new RightClickAction(PluginName);

	private CursorLeftAction CursorLeft = new CursorLeftAction(PluginName);

	private CursorRightAction CursorRight = new CursorRightAction(PluginName);

	private CursorUpAction CursorUp = new CursorUpAction(PluginName);

	private CursorDownAction CursorDown = new CursorDownAction(PluginName);

	public override string Name => PluginName;

	public override string Description => "Controls Mouse Movements and Clicks";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions => new IComputerAction[8] { LeftClick, DoubleClick, MiddleClick, RightClick, CursorLeft, CursorRight, CursorUp, CursorDown };

	public MouseActionsPlugin()
		: base(Resources.Mouse)
	{
	}
}
