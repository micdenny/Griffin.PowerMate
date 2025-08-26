using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace Griffin.Appcasting;

public class AppcastUpdater : IDisposable
{
	private enum HRESULT : uint
	{
		S_OK = 0u,
		E_FAIL = 2147500037u,
		E_INVALIDARG = 2147942487u
	}

	private const int RESTART_MAX_CMD_LINE = 2048;

	private string _CurrentBuild;

	private TimeSpan _CheckInterval;

	private DateTime _LastUpdateCheck = DateTime.Now;

	private AppcastFeed _AppcastFeed;

	private System.Timers.Timer UpdateTimer = new System.Timers.Timer();

	private WebClient DownloadClient = new WebClient();

	private Control Synchronizer = new Control();

	private AppcastUpdaterDownloadEventArgs _CurrentDownloadEventArgs;

	private bool _IsCheckingForUpdate;

	private bool IsDisposed;

	private bool _Downloading;

	public TimeSpan CheckInterval
	{
		get
		{
			return _CheckInterval;
		}
		set
		{
			_CheckInterval = value.Duration();
			SetUpdateTimer();
		}
	}

	public DateTime LastUpdateCheck
	{
		get
		{
			return _LastUpdateCheck;
		}
		set
		{
			if (value != LastUpdateCheck)
			{
				_LastUpdateCheck = value;
				SetUpdateTimer();
			}
		}
	}

	public AppcastFeed AppcastFeed
	{
		get
		{
			return _AppcastFeed;
		}
		set
		{
			_AppcastFeed = value;
		}
	}

	public string CurrentBuild
	{
		get
		{
			return _CurrentBuild;
		}
		set
		{
			_CurrentBuild = value;
		}
	}

	public bool IsCheckingForUpdate => _IsCheckingForUpdate;

	public bool Downloading => _Downloading;

	public AppcastItem DownloadingItem
	{
		get
		{
			if (_CurrentDownloadEventArgs != null)
			{
				return CurrentDownloadEventArgs.Item;
			}
			return null;
		}
	}

	protected AppcastUpdaterDownloadEventArgs CurrentDownloadEventArgs => _CurrentDownloadEventArgs;

	public event EventHandler<CheckingForUpdateEventArgs> CheckingForUpdate;

	public event EventHandler<CheckedForUpdateEventArgs> CheckedForUpdate;

	public event EventHandler<UpdateAvailableEventArgs> UpdateAvailable;

	public event EventHandler<AppcastUpdaterDownloadEventArgs> DownloadStarted;

	public event EventHandler<AppcastUpdaterDownloadEventArgs> DonwloadCompleted;

	public event EventHandler<AppcastUpdaterDownloadEventArgs> DownloadCancelled;

	public event EventHandler<AppcastUpdaterDownloadEventArgs> DownloadFailed;

	public event DownloadProgressChangedEventHandler DownloadProgressChanged;

	public event EventHandler Disposed;

	public AppcastUpdater(string currentBuild)
		: this(currentBuild, null)
	{
	}

	public AppcastUpdater(string currentBuild, AppcastFeed appcastFeed)
		: this(currentBuild, appcastFeed, TimeSpan.Zero)
	{
	}

	public AppcastUpdater(string currentBuild, AppcastFeed appcastFeed, TimeSpan checkInterval)
		: this(currentBuild, appcastFeed, checkInterval, DateTime.Now)
	{
	}

	public AppcastUpdater(string currentBuild, AppcastFeed appcastFeed, TimeSpan checkInterval, DateTime lastUpdateCheck)
	{
		UpdateTimer.AutoReset = false;
		UpdateTimer.Elapsed += UpdateTimer_Elapsed;
		_ = Synchronizer.Handle;
		UpdateTimer.SynchronizingObject = Synchronizer;
		DownloadClient.DownloadFileCompleted += DownloadClient_DownloadFileCompleted;
		DownloadClient.DownloadProgressChanged += DownloadClient_DownloadProgressChanged;
		CurrentBuild = currentBuild;
		AppcastFeed = appcastFeed;
		_LastUpdateCheck = lastUpdateCheck;
		CheckInterval = checkInterval;
	}

	[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
	private static extern HRESULT RegisterApplicationRestart(string commandLine, RESTART_Flags flags);

	[DllImport("Kernel32.dll")]
	private static extern HRESULT UnregisterApplicationRestart();

	public void VistaRegisterApplicationRestart(string commandLine, RESTART_Flags flags)
	{
		if (Environment.OSVersion.Version.Major >= 6)
		{
			HRESULT hRESULT = RegisterApplicationRestart(commandLine, flags);
			if (hRESULT != HRESULT.S_OK)
			{
				throw new Win32Exception((int)hRESULT, hRESULT.ToString());
			}
		}
	}

	public void VistaUnregisterApplicationRestart()
	{
		if (Environment.OSVersion.Version.Major >= 6)
		{
			HRESULT hRESULT = UnregisterApplicationRestart();
			if (hRESULT != HRESULT.S_OK)
			{
				throw new Win32Exception((int)hRESULT, hRESULT.ToString());
			}
		}
	}

	public AppcastItem CheckForUpdate()
	{
		return CheckForUpdate(auto: false);
	}

	public void StartDownload(AppcastItem item, string destination)
	{
		if (string.IsNullOrEmpty(destination))
		{
			destination = Path.GetTempPath() + "\\" + GetFilenameFromPath(item.Location);
		}
		if (Downloading)
		{
			CancelDownload();
		}
		_Downloading = true;
		_CurrentDownloadEventArgs = new AppcastUpdaterDownloadEventArgs(item, destination);
		try
		{
			DownloadClient.Headers.Set("user-agent", AppcastSettings.UserAgent);
			DownloadClient.DownloadFileAsync(new Uri(item.Location), destination);
			OnDownloadStarted(_CurrentDownloadEventArgs);
		}
		catch
		{
			_Downloading = false;
			OnDownloadFailed(_CurrentDownloadEventArgs);
		}
	}

	public void CancelDownload()
	{
		if (Downloading)
		{
			DownloadClient.CancelAsync();
		}
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			if (Downloading)
			{
				CancelDownload();
			}
			UpdateTimer.Dispose();
			DownloadClient.Dispose();
			OnDisposed(EventArgs.Empty);
		}
	}

	protected virtual void OnCheckingForUpdate(CheckingForUpdateEventArgs e)
	{
		if (this.CheckingForUpdate != null)
		{
			this.CheckingForUpdate(this, e);
		}
	}

	protected virtual void OnCheckedForUpdate(CheckedForUpdateEventArgs e)
	{
		if (this.CheckedForUpdate != null)
		{
			this.CheckedForUpdate(this, e);
		}
	}

	protected virtual void OnUpdateAvailable(UpdateAvailableEventArgs e)
	{
		if (this.UpdateAvailable != null)
		{
			this.UpdateAvailable(this, e);
		}
	}

	protected virtual void OnDownloadStarted(AppcastUpdaterDownloadEventArgs e)
	{
		if (this.DownloadStarted != null)
		{
			this.DownloadStarted(this, e);
		}
	}

	protected virtual void OnDownloadCompleted(AppcastUpdaterDownloadEventArgs e)
	{
		if (this.DonwloadCompleted != null)
		{
			this.DonwloadCompleted(this, e);
		}
	}

	protected virtual void OnDownloadCancelled(AppcastUpdaterDownloadEventArgs e)
	{
		if (this.DownloadCancelled != null)
		{
			this.DownloadCancelled(this, e);
		}
	}

	protected virtual void OnDownloadFailed(AppcastUpdaterDownloadEventArgs e)
	{
		if (this.DownloadFailed != null)
		{
			this.DownloadFailed(this, e);
		}
	}

	protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
	{
		if (this.DownloadProgressChanged != null)
		{
			this.DownloadProgressChanged(this, e);
		}
	}

	protected virtual void OnDisposed(EventArgs e)
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, e);
		}
	}

	protected static string GetFilenameFromPath(string location)
	{
		string text = null;
		int num = location.LastIndexOf('/') + 1;
		if (num < 0)
		{
			num = location.LastIndexOf('\\') + 1;
		}
		if (num >= 0)
		{
			return location.Substring(num, location.Length - num);
		}
		return location;
	}

	protected virtual AppcastItem CheckForUpdate(bool auto)
	{
		AppcastItem appcastItem = null;
		_IsCheckingForUpdate = true;
		UpdateTimer.Stop();
		CheckingForUpdateEventArgs e = new CheckingForUpdateEventArgs(auto);
		OnCheckingForUpdate(e);
		if (e.CheckForUpdate)
		{
			if (AppcastFeed != null)
			{
				AppcastFeed.Reload();
				appcastItem = AppcastFeed.GreatestBuildItem;
				if (appcastItem != null && appcastItem.CompareBuild(CurrentBuild) > 0)
				{
					UpdateAvailableEventArgs e2 = new UpdateAvailableEventArgs(appcastItem, auto);
					OnUpdateAvailable(e2);
					if (e2.Download)
					{
						StartDownload(appcastItem, e2.Destination);
					}
				}
				else
				{
					appcastItem = null;
				}
			}
			_IsCheckingForUpdate = false;
			_LastUpdateCheck = DateTime.Now;
			OnCheckedForUpdate(new CheckedForUpdateEventArgs(appcastItem, _LastUpdateCheck, auto));
		}
		SetUpdateTimer();
		return appcastItem;
	}

	private void SetUpdateTimer()
	{
		if (CheckInterval != TimeSpan.Zero && !IsCheckingForUpdate && !Downloading)
		{
			TimeSpan timeSpan = DateTime.Now - LastUpdateCheck;
			if (CheckInterval > timeSpan)
			{
				UpdateTimer.Interval = (CheckInterval - timeSpan).TotalMilliseconds;
				UpdateTimer.Start();
			}
			else
			{
				CheckForUpdate(auto: true);
			}
		}
		else
		{
			UpdateTimer.Stop();
		}
	}

	private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (!IsCheckingForUpdate && !Downloading)
		{
			CheckForUpdate(auto: true);
		}
	}

	private void DownloadClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
	{
		_Downloading = false;
		_CurrentDownloadEventArgs = null;
		if (e.Cancelled)
		{
			OnDownloadCancelled(_CurrentDownloadEventArgs);
		}
		else
		{
			OnDownloadCompleted(_CurrentDownloadEventArgs);
		}
		SetUpdateTimer();
	}

	private void DownloadClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		OnDownloadProgressChanged(e);
	}
}
