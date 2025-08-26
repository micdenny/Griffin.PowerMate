using System;

namespace Griffin.Appcasting;

public class CheckingForUpdateEventArgs : EventArgs
{
	private bool _CheckForUpdate = true;

	private bool _IsAutoCheck;

	public bool CheckForUpdate
	{
		get
		{
			return _CheckForUpdate;
		}
		set
		{
			_CheckForUpdate = value;
		}
	}

	public bool IsAutoCheck => _IsAutoCheck;

	public CheckingForUpdateEventArgs(bool isAutoCheck)
	{
		_IsAutoCheck = isAutoCheck;
	}
}
