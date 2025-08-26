using System;

namespace Griffin.PowerMate.App;

public class PowerMateNodeEventArgs : EventArgs
{
	private string Type;

	public string SettingType => Type;

	public PowerMateNodeEventArgs(string type)
	{
		Type = type;
	}
}
