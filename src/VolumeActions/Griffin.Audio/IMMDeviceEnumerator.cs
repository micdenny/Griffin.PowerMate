using System.Runtime.InteropServices;

namespace Griffin.Audio;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
internal interface IMMDeviceEnumerator
{
	void EnumAudioEndpoints(EDataFlow dataFlow, DEVICE_STATE stateMask, out object devices);

	void GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice device);

	void GetDevice(string id, out IMMDevice device);

	void RegisterEndpointNotificationCallback(object notify);

	void UnregisterEndpointNotificationCallback(object notify);
}
