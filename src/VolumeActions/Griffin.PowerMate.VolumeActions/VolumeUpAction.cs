using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.VolumeActions.Properties;

namespace Griffin.PowerMate.VolumeActions;

internal class VolumeUpAction : ComputerActionBase
{
	private PowerMateLedSetterThread LedSetter;

	public override string Name => "Volume Up";

	public override string Description => "Raises System Volume";

	public override Panel Panel => null;

	public VolumeUpAction(string pluginName, PowerMateLedSetterThread ledSetter)
		: base(pluginName, Resources.volumeUp)
	{
		LedSetter = ledSetter;
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Keyboard.KeyPress(Keys.VolumeUp);
		LedSetter.SetLedToVolume(sender);
		return true;
	}
}
