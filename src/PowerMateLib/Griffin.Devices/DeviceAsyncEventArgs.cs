using System;

namespace Griffin.Devices;

internal class DeviceAsyncEventArgs : EventArgs
{
	private byte[] _Buffer;

	public byte[] Buffer => _Buffer;

	public DeviceAsyncEventArgs(byte[] array)
	{
		_Buffer = array;
	}
}
