using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

public class DeviceNode : NodeContainer<AppNode>
{
	public const string XmlNodeName = "device";

	private IPowerMateDevice PMDevice;

	private PowerMateActions PMFilter;

	private EventHandler<PowerMateEventArgs> FilterReports;

	private EventHandler DeviceDetached;

	protected bool IsActive = true;

	private bool IsGlobalOnly;

	public virtual bool Active
	{
		get
		{
			return IsActive;
		}
		set
		{
			if (IsActive != value && HasDevice)
			{
				if (IsActive)
				{
					PMDevice.ReportReceived -= FilterReports;
				}
				else
				{
					PMDevice.ReportReceived += FilterReports;
					InitializePMLight();
				}
			}
			IsActive = value;
		}
	}

	public IPowerMateDevice Device
	{
		get
		{
			return PMDevice;
		}
		internal set
		{
			if (PMDevice != null)
			{
				if (IsActive)
				{
					PMDevice.ReportReceived -= FilterReports;
				}
				PMDevice.Detached -= DeviceDetached;
			}
			bool flag = PMDevice == null != (value == null);
			PMDevice = value;
			if (value != null)
			{
				if (IsActive)
				{
					value.ReportReceived += FilterReports;
					InitializePMLight();
				}
				value.Detached += DeviceDetached;
				SetSetting("lastpath", value.DevicePath);
			}
			if (flag)
			{
				OnHasDeviceChanged(EventArgs.Empty);
			}
		}
	}

	public bool HasDevice
	{
		get
		{
			if (PMDevice != null && PMDevice.Attached)
			{
				return true;
			}
			return false;
		}
	}

	public string Name
	{
		get
		{
			if (NodeXml.HasAttribute("name"))
			{
				return NodeXml.GetAttribute("name");
			}
			return null;
		}
		set
		{
			if (value != null)
			{
				NodeXml.SetAttribute("name", value);
			}
			else
			{
				NodeXml.RemoveAttribute("name");
			}
			OnNodeSettingsChanged(new PowerMateNodeEventArgs("name"));
		}
	}

	public string LastPath => GetSetting("lastpath");

	public uint HoldTime
	{
		get
		{
			try
			{
				return XmlConvert.ToUInt32(GetSetting("hold"));
			}
			catch
			{
				return 1000u;
			}
		}
		set
		{
			SetSetting("hold", value.ToString());
			PMFilter.HoldTime = value;
		}
	}

	public uint DoubleHoldTime
	{
		get
		{
			try
			{
				return XmlConvert.ToUInt32(GetSetting("doublehold"));
			}
			catch
			{
				return 1000u;
			}
		}
		set
		{
			SetSetting("doublehold", value.ToString());
			PMFilter.DoubleHoldTime = value;
		}
	}

	public uint DoubleClickTime
	{
		get
		{
			try
			{
				return XmlConvert.ToUInt32(GetSetting("doubleclick"));
			}
			catch
			{
				return 0u;
			}
		}
		set
		{
			SetSetting("doubleclick", value.ToString());
			PMFilter.DoubleClickTime = value;
		}
	}

	public bool GlobalOnly
	{
		get
		{
			try
			{
				return XmlConvert.ToBoolean(GetSetting("globalOnly").ToLower());
			}
			catch
			{
				return false;
			}
		}
		set
		{
			if (value != IsGlobalOnly)
			{
				SetSetting("globalOnly", value.ToString());
				IsGlobalOnly = value;
			}
		}
	}

	public bool PulseDuringSleep
	{
		get
		{
			try
			{
				return XmlConvert.ToBoolean(GetSetting("sleepPulse").ToLower());
			}
			catch
			{
				return false;
			}
		}
		set
		{
			SetSetting("sleepPulse", value.ToString());
			if (HasDevice)
			{
				PMDevice.PulseDuringSleep = value;
			}
		}
	}

	public bool Pulse
	{
		get
		{
			try
			{
				return XmlConvert.ToBoolean(GetSetting("pulse").ToLower());
			}
			catch
			{
				return false;
			}
		}
		set
		{
			SetSetting("pulse", value.ToString());
			if (HasDevice)
			{
				PMDevice.Pulse = value;
			}
		}
	}

	public byte PulseRate
	{
		get
		{
			try
			{
				return XmlConvert.ToByte(GetSetting("pulseRate"));
			}
			catch
			{
				return 6;
			}
		}
		set
		{
			if (value > 24)
			{
				value = 24;
			}
			SetSetting("pulseRate", value.ToString());
			if (HasDevice)
			{
				PMDevice.PulseSpeed = value;
			}
		}
	}

	public byte LEDBrightness
	{
		get
		{
			try
			{
				return XmlConvert.ToByte(GetSetting("ledBrightness"));
			}
			catch
			{
				return byte.MaxValue;
			}
		}
		set
		{
			SetSetting("ledBrightness", value.ToString());
			if (HasDevice)
			{
				try
				{
					PMDevice.LEDBrightness = value;
				}
				catch
				{
					ShowDeviceCommunicationErrorMessage();
				}
			}
		}
	}

	public static Process ForegroundProcess
	{
		get
		{
			int ProcessId = 0;
			GetWindowThreadProcessId(GetForegroundWindow(), ref ProcessId);
			return Process.GetProcessById(ProcessId);
		}
	}

	protected AppNode ActiveImage
	{
		get
		{
			AppNode appNode = null;
			if (!IsGlobalOnly)
			{
				Process foregroundProcess = ForegroundProcess;
				appNode = Find(foregroundProcess.ProcessName, caseSensitive: false);
				ForegroundProcess.Dispose();
			}
			if (appNode == null)
			{
				appNode = Find("", caseSensitive: false);
			}
			return appNode;
		}
	}

	public override string TagName => "device";

	public event EventHandler HasDeviceChanged;

	public DeviceNode()
	{
		InitPMFilter();
		DeviceDetached = DeviceNode_PowerMateDetached;
	}

	internal DeviceNode(IPowerMateDevice powermate)
		: this()
	{
		Device = powermate;
	}

	public DeviceNode(XmlElement elem, IPMActionPlugin[] plugins)
		: base(elem)
	{
		InitPMFilter();
		DeviceDetached = DeviceNode_PowerMateDetached;
		IsGlobalOnly = GlobalOnly;
		while (elem["app"] != null)
		{
			Add(new AppNode((XmlElement)elem.RemoveChild(elem["app"]), plugins));
		}
	}

	internal DeviceNode(XmlElement elem, IPowerMateDevice powermate, IPMActionPlugin[] plugins)
		: this(elem, plugins)
	{
		Device = powermate;
	}

	private void InitPMFilter()
	{
		PMFilter = new PowerMateActions();
		PMFilter.HoldTime = HoldTime;
		PMFilter.DoubleHoldTime = DoubleHoldTime;
		PMFilter.DoubleClickTime = DoubleClickTime;
		PMFilter.Click += Click;
		PMFilter.TimedClick += TimedClick;
		PMFilter.ClockwiseRotate += ClockwiseRotate;
		PMFilter.CounterClockwiseRotate += CounterClockwiseRotate;
		PMFilter.ClickClockwiseRotate += ClickClockwiseRotate;
		PMFilter.ClickCounterClockwiseRotate += ClickCounterClockwiseRotate;
		PMFilter.DoubleClick += DoubleClick;
		PMFilter.TimedDoubleClick += TimedDoubleClick;
		PMFilter.DoubleClickClockwiseRotate += DoubleClickClockwiseRotate;
		PMFilter.DoubleClickCounterClockwiseRotate += DoubleClickCounterClockwiseRotate;
		FilterReports = PMFilter.ProcessReportHandler;
	}

	private void InitializePMLight()
	{
		if (HasDevice)
		{
			SetDeviceLED();
			PMDevice.PulseDuringSleep = PulseDuringSleep;
		}
	}

	public void Click(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.Click, e.ModifierKeys);
	}

	public void TimedClick(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.TimedClick, e.ModifierKeys);
	}

	public void ClockwiseRotate(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.ClockwiseRotate, e.ModifierKeys);
	}

	public void CounterClockwiseRotate(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.CounterClockwiseRotate, e.ModifierKeys);
	}

	public void ClickClockwiseRotate(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.ClickClockwiseRotate, e.ModifierKeys);
	}

	public void ClickCounterClockwiseRotate(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.ClickCounterClockwiseRotate, e.ModifierKeys);
	}

	public void DoubleClick(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.DoubleClick, e.ModifierKeys);
	}

	public void TimedDoubleClick(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.TimedDoubleClick, e.ModifierKeys);
	}

	public void DoubleClickClockwiseRotate(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.DoubleClickClockwiseRotate, e.ModifierKeys);
	}

	public void DoubleClickCounterClockwiseRotate(object sender, PMActionEventArgs e)
	{
		PerformPMAction(e.Device, PMAction.DoubleClickCounterClockwiseRotate, e.ModifierKeys);
	}

	private void PerformPMAction(IPowerMateDevice powermate, PMAction pmAction, ModifierKey modifier)
	{
		ActiveImage?.PerformAll(powermate, pmAction, modifier);
	}

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, ref int ProcessId);

	public AppNode Find(string image, bool caseSensitive)
	{
		if (caseSensitive)
		{
			using IEnumerator<AppNode> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				AppNode current = enumerator.Current;
				if (current.Image == image.ToLower())
				{
					return current;
				}
			}
		}
		else
		{
			image = image.ToLower();
			using IEnumerator<AppNode> enumerator2 = GetEnumerator();
			while (enumerator2.MoveNext())
			{
				AppNode current2 = enumerator2.Current;
				if (current2.Image.ToLower() == image.ToLower())
				{
					return current2;
				}
			}
		}
		return null;
	}

	public bool Contains(string image)
	{
		using (IEnumerator<AppNode> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AppNode current = enumerator.Current;
				if (current.Image == image)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override PowerMateNode Clone()
	{
		DeviceNode deviceNode = new DeviceNode();
		deviceNode.NodeXml = (XmlElement)NodeXml.Clone();
		using IEnumerator<AppNode> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			AppNode current = enumerator.Current;
			deviceNode.Add((AppNode)current.Clone());
		}
		return deviceNode;
	}

	public void SetDeviceLED()
	{
		if (!HasDevice)
		{
			return;
		}
		PMDevice.PulseSpeed = PulseRate;
		PMDevice.Pulse = Pulse;
		if (!Pulse)
		{
			try
			{
				PMDevice.LEDBrightness = LEDBrightness;
			}
			catch
			{
				ShowDeviceCommunicationErrorMessage();
			}
		}
	}

	protected virtual void OnHasDeviceChanged(EventArgs e)
	{
		if (this.HasDeviceChanged != null)
		{
			this.HasDeviceChanged(this, e);
		}
	}

	private void DeviceNode_PowerMateDetached(object sender, EventArgs e)
	{
		OnHasDeviceChanged(e);
	}

	protected void ShowDeviceCommunicationErrorMessage()
	{
		MessageBox.Show("There was a problem communicationg with a PowerMate device. Unplugging and reconnecting the PowerMate may correct it.", "PowerMate Device Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}
}
