using System;
using System.Threading;
using Griffin.Audio;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.VolumeActions;

internal class PowerMateLedSetterThread : IDisposable
{
	private Thread LedSetThread;

	private AutoResetEvent SetLedEvent;

	private ManualResetEvent EndThreadEvent;

	private IPowerMateDevice PowerMateDevice;

	private IOSVolume OsVolume;

	public IOSVolume OSVolume
	{
		get
		{
			return OsVolume;
		}
		set
		{
			lock (this)
			{
				OsVolume = value;
			}
		}
	}

	public PowerMateLedSetterThread(IOSVolume volume)
	{
		LedSetThread = new Thread(LedSetThreadMethod);
		SetLedEvent = new AutoResetEvent(initialState: false);
		EndThreadEvent = new ManualResetEvent(initialState: false);
		OsVolume = volume;
		LedSetThread.Start();
	}

	public void SetLedToVolume(IPowerMateDevice powermate)
	{
		lock (this)
		{
			PowerMateDevice = powermate;
		}
		SetLedEvent.Set();
	}

	public void Dispose()
	{
		EndThreadEvent.Set();
	}

	private void LedSetThreadMethod()
	{
		EventWaitHandle[] waitHandles = new EventWaitHandle[2] { SetLedEvent, EndThreadEvent };
		bool flag = false;
		while (!flag)
		{
			switch (WaitHandle.WaitAny(waitHandles))
			{
			case 0:
				if (PowerMateDevice != null && OsVolume != null)
				{
					try
					{
						PowerMateDevice.LEDBrightness = (byte)(OsVolume.MasterVolume * 255f);
					}
					catch
					{
					}
				}
				break;
			case 1:
				flag = true;
				break;
			}
		}
	}
}
