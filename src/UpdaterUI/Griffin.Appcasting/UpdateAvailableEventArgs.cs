namespace Griffin.Appcasting;

public class UpdateAvailableEventArgs : AppcastUpdaterEventArgs
{
	private bool _Download;

	private string _Destination;

	private bool _IsAutoCheck;

	public bool Download
	{
		get
		{
			return _Download;
		}
		set
		{
			_Download = value;
		}
	}

	public string Destination
	{
		get
		{
			return _Destination;
		}
		set
		{
			_Destination = value;
		}
	}

	public bool IsAutoCheck => _IsAutoCheck;

	public UpdateAvailableEventArgs(AppcastItem item, bool isAutoCheck)
		: base(item)
	{
		_IsAutoCheck = isAutoCheck;
	}
}
