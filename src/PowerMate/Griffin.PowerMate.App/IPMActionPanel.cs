using System;

namespace Griffin.PowerMate.App;

public interface IPMActionPanel
{
	string[] Settings { get; set; }

	event EventHandler UpdateSettings;
}
