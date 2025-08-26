using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.VolumeActions.Properties;

namespace Griffin.PowerMate.VolumeActions;

internal class VolumeDownAction : ComputerActionBase
{
	private PowerMateLedSetterThread LedSetter;

	public override string Name => "Volume Down";

	public override string Description => "Lowers System Volume";

	public override Panel Panel => null;

	public VolumeDownAction(string pluginName, PowerMateLedSetterThread ledSetter)
		: base(pluginName, Resources.volumeDown)
	{
		LedSetter = ledSetter;
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		Keyboard.KeyPress(Keys.VolumeDown);
		LedSetter.SetLedToVolume(sender);
		return true;
	}
}
