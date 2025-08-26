using Griffin.PowerMate.App;
using Griffin.PowerMate.OpenActions.Properties;

namespace Griffin.PowerMate.OpenActions;

public class OpenActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "Open File";

	private OpenAction Open = new OpenAction(PluginName);

	public override string Name => PluginName;

	public override string Description => "Opens an application, file, or web address";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions => new IComputerAction[1] { Open };

	public OpenActionsPlugin()
		: base(Resources.openFile)
	{
	}
}
