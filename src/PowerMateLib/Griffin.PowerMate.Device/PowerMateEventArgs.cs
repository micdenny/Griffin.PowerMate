using System;

namespace Griffin.PowerMate.Device;

public class PowerMateEventArgs : EventArgs
{
	private PowerMateReport _Report;

	public PowerMateReport Report => _Report;

	public PowerMateEventArgs(PowerMateReport report)
	{
		_Report = report;
	}
}
