using System;
using System.Diagnostics;
using System.Reflection;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.UpdateUI;

public class PMFeedbackUI : IPowerMateUIPlugin
{
	public string Name => "Provide Feedback";

	public string Description => "Opens a user's default email client to send feedback regarding the PowerMate to Griffin Technology";

	public string Author => "Griffin Technology";

	public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

	public UIStatus Status => UIStatus.Closed;

	public PowerMateDoc PowerMateDoc
	{
		set
		{
		}
	}

	private string OsSignifier
	{
		get
		{
			string text = "Windows";
			string text2 = Environment.OSVersion.Version.ToString(2);
			return text2 switch
			{
				"4.10" => text + "98", 
				"5.0" => text + "2000", 
				"5.1" => text + "XP", 
				"5.2" => text + "Server2003", 
				"6.0" => text + "Vista", 
				_ => text + text2, 
			} + "(" + IntPtr.Size * 8 + ")";
		}
	}

	public event EventHandler StatusChanged;

	public void Open(PowerMateDoc powerMateDoc)
	{
		Process process = new Process();
		try
		{
			process.StartInfo.UseShellExecute = true;
			process.StartInfo.FileName = "mailto:software@griffintechnology.com?subject=[FEEDBACK PowerMate " + PowerMateApp.Version + " " + PowerMateApp.Build + " " + OsSignifier + "]";
			process.Start();
		}
		finally
		{
			process.Dispose();
		}
	}

	public void Close()
	{
	}
}
