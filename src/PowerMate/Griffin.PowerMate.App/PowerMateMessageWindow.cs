using System;
using System.Windows.Forms;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

internal class PowerMateMessageWindow : NativeWindow
{
	private const int WM_DEVICECHANGE = 537;

	private const int DBT_DEVNODES_CHANGED = 7;

	private const int WM_QUERYENDESSION = 17;

	private const int WM_ENDSESSION = 22;

	private const int ENDSESSION_CLOSEAPP = 1;

	private readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

	private bool InDeviceChange;

	private int DevicesFound;

	public event EventHandler DeviceNumberChanged;

	public event EventHandler DeviceAdded;

	public event EventHandler DeviceRemoved;

	public PowerMateMessageWindow()
	{
		CreateHandle(new CreateParams
		{
			Parent = IntPtr.Zero
		});
	}

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 537 && (int)m.WParam == 7 && !InDeviceChange)
		{
			InDeviceChange = true;
			lock (this)
			{
				int num = HIDPowerMate.NumberAttached + USBPowerMate.NumberAttached;
				if (DevicesFound != num)
				{
					OnDeviceNumberChanged(EventArgs.Empty);
					if (DevicesFound > num)
					{
						OnDeviceRemoved(EventArgs.Empty);
					}
					else if (DevicesFound < num)
					{
						OnDeviceAdded(EventArgs.Empty);
					}
					DevicesFound = num;
				}
			}
			InDeviceChange = false;
		}
		base.WndProc(ref m);
	}

	protected void OnDeviceNumberChanged(EventArgs e)
	{
		if (this.DeviceNumberChanged != null)
		{
			this.DeviceNumberChanged(this, e);
		}
	}

	protected void OnDeviceAdded(EventArgs e)
	{
		if (this.DeviceAdded != null)
		{
			this.DeviceAdded(this, e);
		}
	}

	protected void OnDeviceRemoved(EventArgs e)
	{
		if (this.DeviceRemoved != null)
		{
			this.DeviceRemoved(this, e);
		}
	}
}
