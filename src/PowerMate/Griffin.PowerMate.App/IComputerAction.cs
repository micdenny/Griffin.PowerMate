using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

public interface IComputerAction
{
	string Name { get; }

	Icon Icon { get; }

	string Description { get; }

	Panel Panel { get; }

	string PluginName { get; }

	bool SupportsPMAction(PMAction action);

	bool Perform(IPowerMateDevice sender, params string[] settings);
}
