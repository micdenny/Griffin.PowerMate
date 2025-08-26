using System;

namespace Griffin.PowerMate.Device;

public interface IPowerMateDevice
{
	string Name { get; }

	string DevicePath { get; }

	bool Attached { get; }

	byte LEDBrightness { get; set; }

	bool Pulse { get; set; }

	byte PulseSpeed { get; set; }

	bool PulseDuringSleep { get; set; }

	bool Pressed { get; }

	event EventHandler Detached;

	event EventHandler<PowerMateEventArgs> ReportReceived;
}
