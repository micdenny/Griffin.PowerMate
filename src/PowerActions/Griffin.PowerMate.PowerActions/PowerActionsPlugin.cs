using Griffin.PowerMate.App;
using Griffin.PowerMate.PowerActions.Properties;

namespace Griffin.PowerMate.PowerActions;

public class PowerActionsPlugin : PMActionPluginBase
{
	private static string PluginName = "Power";

	private StandByAction StandBy = new StandByAction(PluginName);

	private HibernateAction Hibernate = new HibernateAction(PluginName);

	public override string Name => PluginName;

	public override string Description => "Computer Power Events";

	public override string Author => "Griffin Technology";

	public override IComputerAction[] AvailableActions => new IComputerAction[2] { StandBy, Hibernate };

	public PowerActionsPlugin()
		: base(Resources.powercfg)
	{
	}
}
