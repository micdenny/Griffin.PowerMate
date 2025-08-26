using System;

namespace Griffin.PowerMate.Device;

public class PowerMateReport
{
	private const int ReportLength = 7;

	private byte[] Report;

	private DateTime _Time;

	public bool Pressed
	{
		get
		{
			if (Report != null && Report[1] == 1)
			{
				return true;
			}
			return false;
		}
	}

	public RotateDirection Rotation
	{
		get
		{
			if (Report == null || Report[2] == 0)
			{
				return RotateDirection.None;
			}
			if (Report[2] >> 7 == 1)
			{
				return RotateDirection.CounterClockwise;
			}
			return RotateDirection.Clockwise;
		}
	}

	public byte LEDBrightness
	{
		get
		{
			if (Report == null)
			{
				return 0;
			}
			return Report[4];
		}
	}

	public bool Pulse
	{
		get
		{
			if (Report != null && (Report[5] & 1) == 1)
			{
				return true;
			}
			return false;
		}
	}

	public byte PulseSpeed
	{
		get
		{
			byte result = 0;
			if (Report != null)
			{
				switch (Report[5] >> 4)
				{
				case 0:
					result = (byte)(7 - Report[6] / 2);
					break;
				case 1:
					result = 8;
					break;
				case 2:
					result = (byte)(Report[6] / 2 + 8);
					break;
				}
			}
			return result;
		}
	}

	public bool PulseDuringSleep
	{
		get
		{
			if (Report != null && (Report[5] & 4) == 4)
			{
				return true;
			}
			return false;
		}
	}

	public DateTime Time => _Time;

	public PowerMateReport(byte[] report)
	{
		if (report != null && report.Length == 7)
		{
			Report = report;
		}
		_Time = DateTime.Now;
	}
}
