using System;
using System.IO;
using Griffin.Devices;

namespace Griffin.PowerMate.Device;

public class HIDPowerMate : IPowerMateDevice, IDisposable
{
	private const ushort VendorID = 1917;

	private const ushort ProductID = 1040;

	private const string PnpId = "HID\\Vid_077d&Pid_0410";

	private HID PowerMateHID;

	private bool Disposed;

	public string Name
	{
		get
		{
			if (Attached)
			{
				return PowerMateHID.ProductString;
			}
			return null;
		}
	}

	public string Manufacturer
	{
		get
		{
			if (Attached)
			{
				return PowerMateHID.ManufacturerString;
			}
			return null;
		}
	}

	public string DevicePath
	{
		get
		{
			if (PowerMateHID != null)
			{
				return PowerMateHID.DevicePath;
			}
			return null;
		}
	}

	public bool Attached
	{
		get
		{
			if (PowerMateHID != null)
			{
				return PowerMateHID.IsOpen;
			}
			return false;
		}
	}

	public byte LEDBrightness
	{
		get
		{
			return new PowerMateReport(PowerMateHID.Report).LEDBrightness;
		}
		set
		{
			if (Attached)
			{
				byte[] array = new byte[PowerMateHID.Capabilities.FeatureReportByteLength];
				array[0] = 0;
				array[1] = 65;
				array[2] = 1;
				array[3] = 1;
				array[4] = 0;
				array[5] = value;
				try
				{
					PowerMateHID.Feature = array;
				}
				catch
				{
				}
			}
		}
	}

	public bool Pulse
	{
		get
		{
			return new PowerMateReport(PowerMateHID.Report).Pulse;
		}
		set
		{
			if (Attached)
			{
				byte[] array = new byte[PowerMateHID.Capabilities.FeatureReportByteLength];
				array[0] = 0;
				array[1] = 65;
				array[2] = 1;
				array[3] = 3;
				array[4] = 0;
				if (value)
				{
					array[5] = 1;
				}
				else
				{
					array[5] = 0;
				}
				try
				{
					PowerMateHID.Feature = array;
				}
				catch
				{
				}
			}
		}
	}

	public byte PulseSpeed
	{
		get
		{
			return new PowerMateReport(PowerMateHID.Report).PulseSpeed;
		}
		set
		{
			if (Attached && value <= 24)
			{
				byte[] array = new byte[PowerMateHID.Capabilities.FeatureReportByteLength];
				array[0] = 0;
				array[1] = 65;
				array[2] = 1;
				array[3] = 4;
				array[4] = 0;
				if (value < 8)
				{
					array[5] = 0;
					array[6] = (byte)((7 - value) * 2);
				}
				else if (value > 8)
				{
					array[5] = 2;
					array[6] = (byte)((value - 8) * 2);
				}
				else
				{
					array[5] = 1;
					array[6] = 0;
				}
				try
				{
					PowerMateHID.Feature = array;
				}
				catch
				{
				}
			}
		}
	}

	public bool PulseDuringSleep
	{
		get
		{
			return new PowerMateReport(PowerMateHID.Report).PulseDuringSleep;
		}
		set
		{
			if (Attached)
			{
				byte[] array = new byte[PowerMateHID.Capabilities.FeatureReportByteLength];
				array[0] = 0;
				array[1] = 65;
				array[2] = 1;
				array[3] = 2;
				array[4] = 0;
				if (value)
				{
					array[5] = 1;
				}
				else
				{
					array[5] = 0;
				}
				try
				{
					PowerMateHID.Feature = array;
				}
				catch
				{
				}
			}
		}
	}

	public bool Pressed => new PowerMateReport(PowerMateHID.Report).Pressed;

	public static int NumberAttached => HID.Find("HID\\Vid_077d&Pid_0410").Length;

	public event EventHandler Detached;

	public event EventHandler<PowerMateEventArgs> ReportReceived;

	public HIDPowerMate()
	{
		string[] array = HID.Find("HID\\Vid_077d&Pid_0410");
		if (array.Length > 0)
		{
			InitializeHID(array[0]);
		}
	}

	public HIDPowerMate(string devicePath)
	{
		InitializeHID(devicePath);
	}

	private void InitializeHID(string devicePath)
	{
		PowerMateHID = new HID(devicePath, FileAccess.Read);
		PowerMateHID.Detached += PowerMateHID_Detached;
		PowerMateHID.AsyncReadCompleted += PowerMateHID_AsyncReadCompleted;
		PowerMateHID.AsyncRead();
	}

	public static HIDPowerMate[] GetAll()
	{
		string[] array = HID.Find("HID\\Vid_077d&Pid_0410");
		HIDPowerMate[] array2 = new HIDPowerMate[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = new HIDPowerMate(array[i]);
		}
		return array2;
	}

	public void Dispose()
	{
		if (!Disposed)
		{
			Disposed = true;
			PowerMateHID.Dispose();
		}
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

	private void PowerMateHID_AsyncReadCompleted(object sender, DeviceAsyncEventArgs e)
	{
		OnReportReceived(new PowerMateEventArgs(new PowerMateReport(e.Buffer)));
		PowerMateHID.AsyncRead();
	}

	private void PowerMateHID_Detached(object sender, EventArgs e)
	{
		OnDetached(EventArgs.Empty);
	}
}
