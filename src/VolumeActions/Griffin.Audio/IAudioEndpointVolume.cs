using System;
using System.Runtime.InteropServices;

namespace Griffin.Audio;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
internal interface IAudioEndpointVolume
{
	void RegisterControlChangeNotify(object notify);

	void UnregisterControlChangeNotify(object notify);

	void GetChannelCount(out uint channelCount);

	void SetMasterVolumeLevel(float levelDB, Guid eventContext);

	void SetMasterVolumeLevelScalar(float level, Guid eventContext);

	void GetMasterVolumeLevel(out float levelDB);

	void GetMasterVolumeLevelScalar(out float level);

	void SetChannelVolumeLevel(uint nChannel, float levelDB, Guid eventContext);

	void SetChannelVolumeLevelScalar(uint nChannel, float level, Guid eventContext);

	void GetChannelVolumeLevel(uint nChannel, out float levelDB);

	void GetChannelVolumeLevelScalar(uint nChannel, out float level);

	void SetMute(bool mute, Guid eventContext);

	void GetMute(out bool mute);

	void GetVolumeStepInfo(out uint step, out uint stepCount);

	void VolumeStepUp(Guid eventContext);

	void VolumeStepDown(Guid eventContext);

	void QueryHardwareSupport(out ENDPOINT_HARDWARE_SUPPORT hardwareSupportMask);

	void GetVolumeRange(out float levelMinDB, out float levelMaxDB, out float volumeIncrementDB);
}
