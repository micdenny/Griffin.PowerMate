using System;

namespace Griffin.Appcasting;

public class CheckedForUpdateEventArgs : AppcastUpdaterEventArgs
{
	private DateTime _DateTime;

	private bool _IsAutoCheck;

	public bool UpdateFound
	{
		get
		{
			if (base.Item != null)
			{
				return true;
			}
			return false;
		}
	}

	public DateTime DateTime => _DateTime;

	public bool IsAutoCheck => _IsAutoCheck;

	public CheckedForUpdateEventArgs(AppcastItem item, DateTime dateTime, bool isAutoCheck)
		: base(item)
	{
		_DateTime = dateTime;
		_IsAutoCheck = isAutoCheck;
	}
}
