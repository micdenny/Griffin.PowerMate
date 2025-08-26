using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.VolumeActions.Properties;

namespace Griffin.PowerMate.VolumeActions;

internal class VolumeMuteAction : ComputerActionBase
{
	private PowerMateLedSetterThread LedSetter;

	public override string Name => "Mute";

	public override string Description => "Mute System Volume";

	public override Panel Panel => null;

	public VolumeMuteAction(string pluginName, PowerMateLedSetterThread ledSetter)
		: base(pluginName, Resources.volumeMute)
	{
		LedSetter = ledSetter;
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Keyboard.KeyPress(Keys.VolumeMute);
		return true;
	}
}
