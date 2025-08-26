using System;
using System.IO;
using System.Threading;
using Griffin.Devices;

namespace Griffin.PowerMate.Device;

public class USBPowerMate : IPowerMateDevice, IDisposable
{
	public enum IOCTL_POWERMATE : uint
	{
		SET_READ_TIMEOUT = 2236420u,
		GET_DEVICE_DESCRIPTOR = 2236424u,
		GET_CONFIGURATION_DESCRIPTORS = 2236428u,
		GET_SPECIFIED_DESCRIPTOR = 2236432u,
		GET_STATUSES = 2236436u,
		GET_FRAME_INFO = 2236440u,
		SET_LED_BRIGHTNESS = 2236444u,
		PULSE_DURING_SLEEP = 2236448u,
		PULSE_ALWAYS = 2236452u,
		PULSE_SPEED = 2236456u
	}

	private const string PnpId = "USB\\Vid_077d&Pid_0410";

	private const uint IOCTL_POWERMATE_BASE = 2228224u;

	private static readonly Guid PowerMateGuid = new Guid("FC3DA4B7-1E9D-47f4-A7E3-151B97C163A6");

	private Griffin.Devices.Device PowerMateUSB;

	private PowerMateReport LastReport = new PowerMateReport(new byte[7]);

	private Thread ReadThread;

	private bool Disposed;

	public string Name => "Griffin PowerMate";

	public string Manufacturer => "Griffin Technology, Inc.";

	public string DevicePath
	{
		get
		{
			if (PowerMateUSB != null)
			{
				return PowerMateUSB.DevicePath;
			}
			return null;
		}
	}

	public bool Attached
	{
		get
		{
			if (PowerMateUSB != null)
			{
				return PowerMateUSB.IsOpen;
			}
			return false;
		}
	}

	public byte LEDBrightness
	{
		get
		{
			if (LastReport != null && Attached)
			{
				return LastReport.LEDBrightness;
			}
			return 0;
		}
		set
		{
			SendPmControlCode(IOCTL_POWERMATE.SET_LED_BRIGHTNESS, value);
		}
	}

	public bool Pulse
	{
		get
		{
			if (LastReport != null && Attached)
			{
				return LastReport.Pulse;
			}
			return false;
		}
		set
		{
			ushort input = 0;
			if (value)
			{
				input = 1;
			}
			SendPmControlCode(IOCTL_POWERMATE.PULSE_ALWAYS, input);
		}
	}

	public byte PulseSpeed
	{
		get
		{
			if (LastReport != null && Attached)
			{
				return LastReport.PulseSpeed;
			}
			return 0;
		}
		set
		{
			ushort num = value;
			if (num > 24)
			{
				num = 24;
			}
			SendPmControlCode(IOCTL_POWERMATE.PULSE_SPEED, num);
		}
	}

	public bool PulseDuringSleep
	{
		get
		{
			if (LastReport != null && Attached)
			{
				return LastReport.PulseDuringSleep;
			}
			return false;
		}
		set
		{
			ushort input = 0;
			if (value)
			{
				input = 1;
			}
			SendPmControlCode(IOCTL_POWERMATE.PULSE_DURING_SLEEP, input);
		}
	}

	public bool Pressed
	{
		get
		{
			if (LastReport != null && Attached)
			{
				return LastReport.Pressed;
			}
			return false;
		}
	}

	public static int NumberAttached => Griffin.Devices.Device.Find(PowerMateGuid).Length;

	public event EventHandler Detached;

	public event EventHandler<PowerMateEventArgs> ReportReceived;

	public USBPowerMate()
	{
		string[] array = Griffin.Devices.Device.Find(PowerMateGuid, "USB\\Vid_077d&Pid_0410");
		if (array.Length > 0)
		{
			InitializeUSB(array[0]);
		}
	}

	public USBPowerMate(string devicePath)
	{
		InitializeUSB(devicePath);
	}

	private void InitializeUSB(string devicePath)
	{
		PowerMateUSB = new Griffin.Devices.Device(devicePath, FileAccess.ReadWrite);
		PowerMateUSB.Detached += PowerMateUSB_Detached;
		ReadThread = new Thread(BeginReadLoop);
		ReadThread.Start();
	}

	public static USBPowerMate[] GetAll()
	{
		string[] array = Griffin.Devices.Device.Find(PowerMateGuid, "USB\\Vid_077d&Pid_0410");
		USBPowerMate[] array2 = new USBPowerMate[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = new USBPowerMate(array[i]);
		}
		return array2;
	}

	public void Dispose()
	{
		if (!Disposed)
		{
			Disposed = true;
			StopReadLoop();
			PowerMateUSB.Dispose();
		}
	}

	protected byte[] SendPmControlCode(IOCTL_POWERMATE controlCode, ushort input)
	{
		if (Attached)
		{
			PowerMateUSB.SendIoControlCode((uint)controlCode, BitConverter.GetBytes(input), new byte[0]);
		}
		return null;
	}

	protected virtual void OnDetached(EventArgs e)
	{
		if (this.Detached != null)
		{
			this.Detached(this, e);
		}
	}

	protected virtual void OnReportReceived(PowerMateEventArgs e)
	{
		if (this.ReportReceived != null)
		{
			this.ReportReceived(this, e);
		}
	}

	private void BeginReadLoop()
	{
		while (!Disposed)
		{
			byte[] array = new byte[7];
			try
			{
				PowerMateUSB.Read(array, 1, 6);
				lock (LastReport)
				{
					LastReport = new PowerMateReport(array);
				}
				OnReportReceived(new PowerMateEventArgs(LastReport));
			}
			catch
			{
			}
		}
	}

	private void StopReadLoop()
	{
		if (ReadThread != null && ReadThread.IsAlive)
		{
			ReadThread.Abort();
		}
	}

	private void PowerMateUSB_AsyncReadCompleted(object sender, DeviceAsyncEventArgs e)
	{
		LastReport = new PowerMateReport(e.Buffer);
		OnReportReceived(new PowerMateEventArgs(LastReport));
		byte[] array = new byte[7];
		PowerMateUSB.AsyncRead(array, 1, 6);
	}

	private void PowerMateUSB_Detached(object sender, EventArgs e)
	{
		OnDetached(EventArgs.Empty);
		StopReadLoop();
	}
}
