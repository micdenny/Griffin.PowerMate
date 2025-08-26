using System;
using System.Runtime.InteropServices;

namespace Griffin.Audio;

[ComImport]
[Guid("D666063F-1587-4E43-81F1-B948E807363F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMMDevice
{
	void Activate(ref Guid iid, CLSCTX clsCtx, IntPtr activationParams, out IAudioEndpointVolume iidInterface);

	void OpenPropertyStore(STGM stgmAccess, out object properties);

	void GetId(string id);

	void GetState(DEVICE_STATE state);
}
