using System;

namespace Griffin.PowerMate.Device;

public class PMActionEventArgs : EventArgs
{
	private IPowerMateDevice _Device;

	private ModifierKey _ModifierKeys;

	public IPowerMateDevice Device => _Device;

	public ModifierKey ModifierKeys => _ModifierKeys;

	public PMActionEventArgs(IPowerMateDevice device, ModifierKey modifierKeys)
	{
		_Device = device;
		_ModifierKeys = modifierKeys;
	}
}
