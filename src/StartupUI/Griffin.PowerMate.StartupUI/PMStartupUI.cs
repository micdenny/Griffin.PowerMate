using System;
using System.Reflection;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Microsoft.Win32;

namespace Griffin.PowerMate.StartupUI;

public class PMStartupUI : IPowerMateUIPlugin
{
	private RegistryKey RunRegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);

	private string registryValueName = "PowerMate";

	public string Name => "Run on Startup";

	public string Description => "Sets whether or not to run PowerMate on startup.";

	public string Author => "Griffin Technology";

	public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

	public UIStatus Status
	{
		get
		{
			if (RunRegistryKey.GetValue(registryValueName) == null)
			{
				return UIStatus.Closed;
			}
			return UIStatus.Open;
		}
	}

	public PowerMateDoc PowerMateDoc
	{
		set
		{
		}
	}

	public event EventHandler StatusChanged;

	public void Open(PowerMateDoc powerMateDoc)
	{
		if (Status != UIStatus.Open)
		{
			RunRegistryKey.SetValue(registryValueName, Application.ExecutablePath);
			OnStatusChanged(EventArgs.Empty);
		}
		else
		{
			RunRegistryKey.DeleteValue(registryValueName);
			OnStatusChanged(EventArgs.Empty);
		}
	}

	public void Close()
	{
	}

	protected virtual void OnStatusChanged(EventArgs e)
	{
		if (this.StatusChanged != null)
		{
			this.StatusChanged(this, e);
		}
	}
}
