using Griffin.PowerMate.App;
using Griffin.PowerMate.SendKeysActions.Properties;

namespace Griffin.PowerMate.SendKeysActions;

public class SendKeysActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "Send Keys";

	private SendKeysComputerActions SendKeysAction = new SendKeysComputerActions(PluginName);

	public override string Name => PluginName;

	public override string Description => "Sends User-Defined Key Presses";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions => new IComputerAction[1] { SendKeysAction };

	public SendKeysActionsPlugin()
		: base(Resources.keyPress)
	{
	}
}
