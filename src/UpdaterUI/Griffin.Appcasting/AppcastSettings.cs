using System;

namespace Griffin.Appcasting;

public static class AppcastSettings
{
	public const string Version = "1.0.1";

	private static string _UserAgent;

	public static string UserAgent
	{
		get
		{
			if (_UserAgent == null)
			{
				return DefaultUserAgent;
			}
			return _UserAgent;
		}
		set
		{
			_UserAgent = value;
		}
	}

	public static string DefaultUserAgent
	{
		get
		{
			string text = "(";
			return Environment.OSVersion.Platform switch
			{
				PlatformID.Win32NT => text + "Windows NT", 
				PlatformID.Win32Windows => text + "Windows 95", 
				PlatformID.WinCE => text + "Windows CE", 
				_ => text + Environment.OSVersion.Platform, 
			} + " " + Environment.OSVersion.Version.ToString(2) + ") Griffin.Appcasting/1.0.1";
		}
	}
}
