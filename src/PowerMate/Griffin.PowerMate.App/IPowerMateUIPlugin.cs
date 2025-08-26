using System;

namespace Griffin.PowerMate.App;

public interface IPowerMateUIPlugin
{
	string Name { get; }

	string Description { get; }

	string Author { get; }

	string Version { get; }

	UIStatus Status { get; }

	PowerMateDoc PowerMateDoc { set; }

	event EventHandler StatusChanged;

	void Open(PowerMateDoc powerMateDoc);

	void Close();
}
