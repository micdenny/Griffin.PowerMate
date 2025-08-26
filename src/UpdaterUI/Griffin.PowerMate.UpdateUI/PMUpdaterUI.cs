using System;
using System.Reflection;
using Griffin.Appcasting;
using Griffin.PowerMate.App;
using Griffin.PowerMate.UpdateUI.Properties;

namespace Griffin.PowerMate.UpdateUI;

public class PMUpdaterUI : IPowerMateUIPlugin, IDisposable
{
	protected class UpdaterSettingTypes
	{
		public const string AppcastLocation = "appcastLocation";

		public const string AppcastChannel = "appcastChannel";

		public const string UpdateCheck = "updateCheck";

		public const string LastUpdateCheck = "lastUpdateCheck";

		public const string SkipBuild = "skipBuild";
	}

	protected enum UpdateInterval
	{
		Never = 0,
		Daily = 1,
		Weekly = 7,
		Monthly = 28
	}

	private const string AppcastCommandline = "/feed=";

	private const string BaseAppcastLocation = "http://updates.griffintechnology.com/powermate_win_";

	private UIAppcastUpdater Updater;

	private PowerMateDoc _PowerMateDoc;

	private bool IsDisposed;

	private EventHandler<PowerMateNodeEventArgs> NodeSettingsChangedHandler;

	public string Name => "Check For Update";

	public string Description => "Checks for PowerMate software updates from Griffin Technology.";

	public string Author => "Griffin Technology";

	public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

	public UIStatus Status => UIStatus.Closed;

	public PowerMateDoc PowerMateDoc
	{
		protected get
		{
			return _PowerMateDoc;
		}
		set
		{
			if (_PowerMateDoc != null)
			{
				_PowerMateDoc.NodeSettingsChanged -= NodeSettingsChangedHandler;
			}
			if (value != null)
			{
				value.NodeSettingsChanged += NodeSettingsChangedHandler;
			}
			_PowerMateDoc = value;
			if (!IsDisposed)
			{
				Updater.AppcastFeed = new AppcastFeed(AppcastLocation, AppcastChannel);
				Updater.SkipBuild = SkipBuild;
				Updater.CheckInterval = UpdateCheck;
				Updater.LastUpdateCheck = LastUpdateCheck;
			}
		}
	}

	public string AppcastLocation
	{
		get
		{
			string text = null;
			if (PowerMateDoc != null)
			{
				text = PowerMateDoc.GetSetting("appcastLocation");
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "http://updates.griffintechnology.com/powermate_win_";
				string text2 = PowerMateApp.Version.ToLower();
				text = (text2.Contains("a") ? (text + "alpha.xml") : ((!text2.Contains("b")) ? (text + "release.xml") : (text + "beta.xml")));
			}
			return text;
		}
		protected set
		{
			if (!value.StartsWith("http://", ignoreCase: true, null))
			{
				value = "http://" + value;
			}
			if (PowerMateDoc != null)
			{
				PowerMateDoc.SetSetting("appcastLocation", value);
			}
			Updater.AppcastFeed.Location = value;
		}
	}

	public string AppcastChannel
	{
		get
		{
			string result = null;
			if (PowerMateDoc != null)
			{
				result = PowerMateDoc.GetSetting("appcastChannel");
			}
			return result;
		}
		protected set
		{
			if (PowerMateDoc != null)
			{
				PowerMateDoc.SetSetting("appcastChannel", value);
			}
			Updater.AppcastFeed.Title = value;
		}
	}

	public TimeSpan UpdateCheck
	{
		get
		{
			try
			{
				return new TimeSpan((int)Enum.Parse(typeof(UpdateInterval), PowerMateDoc.GetSetting("updateCheck")), 0, 0, 0);
			}
			catch
			{
				return new TimeSpan(7, 0, 0, 0);
			}
		}
		protected set
		{
			if (PowerMateDoc != null)
			{
				PowerMateDoc.SetSetting("updateCheck", value.ToString());
			}
			Updater.CheckInterval = value;
		}
	}

	public DateTime LastUpdateCheck
	{
		get
		{
			try
			{
				return DateTime.Parse(PowerMateDoc.GetSetting("lastUpdateCheck"));
			}
			catch
			{
				return DateTime.Now.Subtract(UpdateCheck);
			}
		}
		protected set
		{
			if (PowerMateDoc != null)
			{
				PowerMateDoc.SetSetting("lastUpdateCheck", value.ToString());
			}
			Updater.LastUpdateCheck = value;
		}
	}

	public string SkipBuild
	{
		get
		{
			string result = null;
			if (PowerMateDoc != null)
			{
				result = PowerMateDoc.GetSetting("skipBuild");
			}
			return result;
		}
		protected set
		{
			if (PowerMateDoc != null)
			{
				PowerMateDoc.SetSetting("skipBuild", value);
			}
			Updater.SkipBuild = value;
		}
	}

	public event EventHandler StatusChanged;

	public PMUpdaterUI()
	{
		Updater = new UIAppcastUpdater(PowerMateApp.Build);
		Updater.CurrentVersion = PowerMateApp.Version;
		Updater.ApplicationName = "PowerMate";
		Updater.Image = Resources.powermate;
		Updater.SetUserAgent();
		NodeSettingsChangedHandler = NodeSettingsChanged;
		Updater.CheckedForUpdate += Updater_CheckedForUpdate;
		PowerMateDoc = PowerMateApp.PowerMateDoc;
		Updater.VistaRegisterApplicationRestart(null, RESTART_Flags.None);
		CheckForAppcastCommandline();
	}

	public void Open(PowerMateDoc powerMateDoc)
	{
		if (powerMateDoc != PowerMateDoc)
		{
			PowerMateDoc = powerMateDoc;
		}
		Updater.CheckForUpdate();
	}

	public void Close()
	{
		if (Updater.Downloading)
		{
			Updater.CancelDownload();
		}
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			if (Updater.Downloading)
			{
				Updater.CancelDownload();
			}
			PowerMateDoc = null;
			Updater.Dispose();
		}
	}

	protected virtual void OnStatusChanged(EventArgs e)
	{
		if (this.StatusChanged != null)
		{
			this.StatusChanged(this, e);
		}
	}

	private bool CheckForAppcastCommandline()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		bool flag = false;
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (flag)
			{
				break;
			}
			if (commandLineArgs[i].StartsWith("/feed=", ignoreCase: true, null))
			{
				string text = commandLineArgs[i].Substring("/feed=".Length).Trim('"', '\'');
				switch (text.ToLower())
				{
				case "b":
				case "beta":
					AppcastLocation = "http://updates.griffintechnology.com/powermate_win_beta.xml";
					break;
				case "r":
				case "release":
					AppcastLocation = "http://updates.griffintechnology.com/powermate_win_release.xml";
					break;
				default:
					AppcastLocation = text;
					break;
				}
				flag = true;
			}
		}
		return flag;
	}

	private void NodeSettingsChanged(object sender, PowerMateNodeEventArgs e)
	{
		if (e.SettingType == "lastUpdateCheck")
		{
			Updater.LastUpdateCheck = LastUpdateCheck;
		}
		else if (e.SettingType == "updateCheck")
		{
			Updater.CheckInterval = UpdateCheck;
		}
		else if (e.SettingType == "appcastLocation")
		{
			Updater.AppcastFeed.Location = AppcastLocation;
		}
		else if (e.SettingType == "appcastChannel")
		{
			Updater.AppcastFeed.Title = AppcastChannel;
		}
	}

	private void Updater_CheckedForUpdate(object sender, CheckedForUpdateEventArgs e)
	{
		LastUpdateCheck = e.DateTime;
		SkipBuild = Updater.SkipBuild;
	}
}
