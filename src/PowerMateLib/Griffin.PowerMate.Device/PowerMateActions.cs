using System;
using System.Timers;
using System.Windows.Forms;

namespace Griffin.PowerMate.Device;

public class PowerMateActions
{
	private enum ClickState
	{
		None,
		Click,
		BetweenDoubleClick,
		DoubleClick,
		ClickAndRotate,
		DoubleClickAndRotate
	}

	private uint HoldMS = 1000u;

	private uint DoubleHoldMS = 1000u;

	private uint DoubleClickMS;

	private uint PMSensitivity;

	private uint RotateReportsReceived;

	private RotateDirection SensitivityDirection = RotateDirection.None;

	private IPowerMateDevice _PowerMate;

	private System.Timers.Timer ClickTimer;

	private IPowerMateDevice ClickPM;

	private ClickState State;

	private EventHandler<PowerMateEventArgs> _ProcessReportHandler;

	public EventHandler<PowerMateEventArgs> ProcessReportHandler => _ProcessReportHandler;

	public uint HoldTime
	{
		get
		{
			return HoldMS;
		}
		set
		{
			HoldMS = value;
		}
	}

	public uint DoubleHoldTime
	{
		get
		{
			return DoubleHoldMS;
		}
		set
		{
			DoubleHoldMS = value;
		}
	}

	public uint DoubleClickTime
	{
		get
		{
			return DoubleClickMS;
		}
		set
		{
			DoubleClickMS = value;
		}
	}

	public uint RotateSensitivity
	{
		get
		{
			return PMSensitivity;
		}
		set
		{
			PMSensitivity = value;
		}
	}

	public IPowerMateDevice PowerMate
	{
		get
		{
			return _PowerMate;
		}
		set
		{
			if (_PowerMate != null)
			{
				_PowerMate.ReportReceived -= ProcessReportHandler;
			}
			_PowerMate = value;
			if (value != null)
			{
				value.ReportReceived += ProcessReportHandler;
			}
		}
	}

	public event EventHandler<PMActionEventArgs> Click;

	public event EventHandler<PMActionEventArgs> TimedClick;

	public event EventHandler<PMActionEventArgs> ClockwiseRotate;

	public event EventHandler<PMActionEventArgs> CounterClockwiseRotate;

	public event EventHandler<PMActionEventArgs> ClickClockwiseRotate;

	public event EventHandler<PMActionEventArgs> ClickCounterClockwiseRotate;

	public event EventHandler<PMActionEventArgs> DoubleClick;

	public event EventHandler<PMActionEventArgs> TimedDoubleClick;

	public event EventHandler<PMActionEventArgs> DoubleClickClockwiseRotate;

	public event EventHandler<PMActionEventArgs> DoubleClickCounterClockwiseRotate;

	public PowerMateActions()
	{
		State = ClickState.None;
		ClickTimer = new System.Timers.Timer();
		ClickTimer.AutoReset = false;
		ClickTimer.Enabled = false;
		ClickTimer.Elapsed += ClickTimer_Elapsed;
		_ProcessReportHandler = ProcessReport;
	}

	public PowerMateActions(IPowerMateDevice powermate)
		: this()
	{
		PowerMate = powermate;
	}

	public void ProcessReport(IPowerMateDevice powermate, PowerMateReport report)
	{
		switch (State)
		{
		case ClickState.None:
			ProcessNone(powermate, report);
			break;
		case ClickState.Click:
			ProcessClick(powermate, report);
			break;
		case ClickState.BetweenDoubleClick:
			ProcessBetweenDoubleClick(powermate, report);
			break;
		case ClickState.DoubleClick:
			ProcessDoubleClick(powermate, report);
			break;
		case ClickState.ClickAndRotate:
			ProcessClickAndRotate(powermate, report);
			break;
		case ClickState.DoubleClickAndRotate:
			ProcessDoubleClickAndRotate(powermate, report);
			break;
		}
	}

	private void ProcessNone(IPowerMateDevice powermate, PowerMateReport report)
	{
		if (report.Pressed)
		{
			if (report.Rotation == RotateDirection.None)
			{
				State = ClickState.Click;
				ProcessClick(powermate, report);
			}
			else if (Rotate(report.Rotation))
			{
				State = ClickState.ClickAndRotate;
				ProcessClickAndRotate(powermate, report);
			}
		}
		else if (Rotate(report.Rotation))
		{
			RotateReportsReceived = 0u;
			switch (report.Rotation)
			{
			case RotateDirection.Clockwise:
				TriggerAction(this.ClockwiseRotate, powermate);
				break;
			case RotateDirection.CounterClockwise:
				TriggerAction(this.CounterClockwiseRotate, powermate);
				break;
			default:
				RotateReportsReceived = 0u;
				break;
			}
		}
	}

	private void ProcessClick(IPowerMateDevice powermate, PowerMateReport report)
	{
		if (report.Pressed)
		{
			if (report.Rotation == RotateDirection.None)
			{
				if (HoldMS != 0)
				{
					ClickTimer.Interval = HoldMS;
					ClickPM = powermate;
					ClickTimer.Start();
				}
			}
			else if (Rotate(report.Rotation))
			{
				ClickTimer.Stop();
				State = ClickState.ClickAndRotate;
				ProcessClickAndRotate(powermate, report);
			}
		}
		else
		{
			ClickTimer.Stop();
			if (report.Rotation == RotateDirection.None && DoubleClickMS != 0)
			{
				State = ClickState.BetweenDoubleClick;
				ProcessBetweenDoubleClick(powermate, report);
			}
			else
			{
				State = ClickState.None;
				TriggerAction(this.Click, powermate);
				ProcessNone(powermate, report);
			}
		}
	}

	private void ProcessBetweenDoubleClick(IPowerMateDevice powermate, PowerMateReport report)
	{
		if (report.Pressed)
		{
			ClickTimer.Stop();
			State = ClickState.DoubleClick;
			ProcessDoubleClick(powermate, report);
		}
		else if (report.Rotation == RotateDirection.None)
		{
			ClickTimer.Interval = DoubleClickMS;
			ClickPM = powermate;
			ClickTimer.Start();
		}
		else
		{
			ClickTimer.Stop();
			State = ClickState.None;
			TriggerAction(this.Click, powermate);
			ProcessNone(powermate, report);
		}
	}

	private void ProcessDoubleClick(IPowerMateDevice powermate, PowerMateReport report)
	{
		if (report.Pressed)
		{
			if (report.Rotation == RotateDirection.None)
			{
				if (DoubleClickMS != 0)
				{
					ClickTimer.Interval = DoubleHoldMS;
					ClickPM = powermate;
					ClickTimer.Start();
				}
			}
			else if (Rotate(report.Rotation))
			{
				ClickTimer.Stop();
				State = ClickState.DoubleClickAndRotate;
				ProcessDoubleClickAndRotate(powermate, report);
			}
		}
		else
		{
			ClickTimer.Stop();
			State = ClickState.None;
			TriggerAction(this.DoubleClick, powermate);
			ProcessNone(powermate, report);
		}
	}

	private void ProcessClickAndRotate(IPowerMateDevice powermate, PowerMateReport report)
	{
		if (!report.Pressed)
		{
			State = ClickState.None;
			ProcessNone(powermate, report);
		}
		else if (Rotate(report.Rotation))
		{
			RotateReportsReceived = 0u;
			switch (report.Rotation)
			{
			case RotateDirection.Clockwise:
				TriggerAction(this.ClickClockwiseRotate, powermate);
				break;
			case RotateDirection.CounterClockwise:
				TriggerAction(this.ClickCounterClockwiseRotate, powermate);
				break;
			}
		}
	}

	private void ProcessDoubleClickAndRotate(IPowerMateDevice powermate, PowerMateReport report)
	{
		if (!report.Pressed)
		{
			State = ClickState.None;
			ProcessNone(powermate, report);
		}
		else if (Rotate(report.Rotation))
		{
			RotateReportsReceived = 0u;
			switch (report.Rotation)
			{
			case RotateDirection.Clockwise:
				TriggerAction(this.DoubleClickClockwiseRotate, powermate);
				break;
			case RotateDirection.CounterClockwise:
				TriggerAction(this.DoubleClickCounterClockwiseRotate, powermate);
				break;
			}
		}
	}

	private void ProcessReport(object sender, PowerMateEventArgs e)
	{
		if (sender is IPowerMateDevice)
		{
			ProcessReport(sender as IPowerMateDevice, e.Report);
		}
	}

	private void ClickTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (State == ClickState.Click)
		{
			State = ClickState.ClickAndRotate;
			TriggerAction(this.TimedClick, ClickPM);
		}
		else if (State == ClickState.BetweenDoubleClick)
		{
			State = ClickState.None;
			TriggerAction(this.Click, ClickPM);
		}
		else if (State == ClickState.DoubleClick)
		{
			State = ClickState.DoubleClickAndRotate;
			TriggerAction(this.TimedDoubleClick, ClickPM);
		}
	}

	private ModifierKey CheckModifierKeys()
	{
		Keys modifierKeys = Control.ModifierKeys;
		ModifierKey modifierKey = ModifierKey.None;
		if ((modifierKeys & Keys.Alt) == Keys.Alt)
		{
			modifierKey |= ModifierKey.Alt;
		}
		if ((modifierKeys & Keys.Control) == Keys.Control)
		{
			modifierKey |= ModifierKey.Ctrl;
		}
		if ((modifierKeys & Keys.Shift) == Keys.Shift)
		{
			modifierKey |= ModifierKey.Shift;
		}
		return modifierKey;
	}

	private bool MSAfter(DateTime time1, DateTime time2, uint ms)
	{
		if ((time1 - time2).TotalMilliseconds >= (double)ms)
		{
			return true;
		}
		return false;
	}

	private bool Rotate(RotateDirection direction)
	{
		if (SensitivityDirection != direction)
		{
			SensitivityDirection = direction;
			RotateReportsReceived = 0u;
		}
		RotateReportsReceived++;
		if (RotateReportsReceived >= PMSensitivity)
		{
			return true;
		}
		return false;
	}

	private bool TriggerAction(EventHandler<PMActionEventArgs> action, IPowerMateDevice powermate)
	{
		if (action != null)
		{
			action(this, new PMActionEventArgs(powermate, CheckModifierKeys()));
			return true;
		}
		return false;
	}
}
