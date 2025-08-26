using System;
using Griffin.PowerMate.App;
using Griffin.PowerMate.ScrollActions.Properties;

namespace Griffin.PowerMate.ScrollActions;

public class ScrollActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "Scrolling";

	private ScrollUpAction ScrollUp = new ScrollUpAction(PluginName);

	private ScrollDownAction ScrollDown = new ScrollDownAction(PluginName);

	private ScrollLeftAction ScrollLeft = new ScrollLeftAction(PluginName);

	private ScrollRightAction ScrollRight = new ScrollRightAction(PluginName);

	public override string Name => PluginName;

	public override string Description => "Controls Window Scrolling";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions
	{
		get
		{
			if (!IsVista)
			{
				return new IComputerAction[2] { ScrollUp, ScrollDown };
			}
			return new IComputerAction[4] { ScrollUp, ScrollDown, ScrollLeft, ScrollRight };
		}
	}

	protected static bool IsVista => Environment.OSVersion.Version.Major >= 6;

	public ScrollActionsPlugin()
		: base(Resources.scroll)
	{
	}

	protected override void OnDisposed(EventArgs e)
	{
		if (!IsVista)
		{
			ScrollLeft.Dispose();
			ScrollRight.Dispose();
		}
		base.OnDisposed(e);
	}
}
