using System;
using System.Runtime.InteropServices;

namespace Griffin.Audio;

internal class VistaVolume : IOSVolume, IDisposable
{
	private static Guid IAudioEndpointVolume_iid = new Guid("5CDF2C82-841E-4546-9722-0CF74078229A");

	private IMMDeviceEnumerator DeviceEnumerator;

	private IMMDevice DefaultDevice;

	private IAudioEndpointVolume EndpointVolume;

	public float MasterVolume
	{
		get
		{
			float level = 0f;
			if (EndpointVolume != null)
			{
				EndpointVolume.GetMasterVolumeLevelScalar(out level);
			}
			return level;
		}
	}

	public bool MasterMute
	{
		get
		{
			bool mute = false;
			if (EndpointVolume != null)
			{
				EndpointVolume.GetMute(out mute);
			}
			return mute;
		}
	}

	public VistaVolume()
	{
		DeviceEnumerator = new MMDeviceEnumerator() as IMMDeviceEnumerator;
		DeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole, out DefaultDevice);
		if (DefaultDevice != null)
		{
			DefaultDevice.Activate(ref IAudioEndpointVolume_iid, CLSCTX.INPROC_SERVER, IntPtr.Zero, out EndpointVolume);
		}
	}

	public void Dispose()
	{
		Marshal.ReleaseComObject(EndpointVolume);
		Marshal.ReleaseComObject(DefaultDevice);
		Marshal.ReleaseComObject(DeviceEnumerator);
	}
}
