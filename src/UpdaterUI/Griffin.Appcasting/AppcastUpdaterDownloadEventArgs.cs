namespace Griffin.Appcasting;

public class AppcastUpdaterDownloadEventArgs : AppcastUpdaterEventArgs
{
	private string _Destination;

	public string Destination => _Destination;

	public AppcastUpdaterDownloadEventArgs(AppcastItem item, string destination)
		: base(item)
	{
		_Destination = destination;
	}
}
