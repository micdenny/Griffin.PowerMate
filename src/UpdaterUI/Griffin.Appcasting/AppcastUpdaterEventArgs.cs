using System;

namespace Griffin.Appcasting;

public class AppcastUpdaterEventArgs : EventArgs
{
	private AppcastItem _Item;

	public AppcastItem Item => _Item;

	public AppcastUpdaterEventArgs(AppcastItem item)
	{
		_Item = item;
	}
}
