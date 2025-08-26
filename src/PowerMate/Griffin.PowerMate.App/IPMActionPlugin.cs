using System.Drawing;

namespace Griffin.PowerMate.App;

public interface IPMActionPlugin
{
	string Name { get; }

	string Description { get; }

	Icon Icon { get; }

	string Author { get; }

	string Version { get; }

	IComputerAction[] AvailableActions { get; }
}
