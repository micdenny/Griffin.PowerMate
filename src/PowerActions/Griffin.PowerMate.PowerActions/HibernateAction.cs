using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.PowerActions.Properties;

namespace Griffin.PowerMate.PowerActions;

internal class HibernateAction : ComputerActionBase
{
	public override string Name => "Hibernate";

	public override string Description => "Activates the Computer's Hibernate Mode";

	public override Panel Panel => null;

	public HibernateAction(string pluginName)
		: base(pluginName, Resources.standBy)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		return Application.SetSuspendState(PowerState.Hibernate, force: false, disableWakeEvent: false);
	}
}
