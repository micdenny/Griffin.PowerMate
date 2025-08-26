using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.PowerActions.Properties;

namespace Griffin.PowerMate.PowerActions;

internal class StandByAction : ComputerActionBase
{
	public override string Name => "Stand By";

	public override string Description => "Activates the Computer's Stand By Mode";

	public override Panel Panel => null;

	public StandByAction(string pluginName)
		: base(pluginName, Resources.standBy)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		return Application.SetSuspendState(PowerState.Suspend, force: false, disableWakeEvent: false);
	}
}
