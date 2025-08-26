using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Griffin.Appcasting;

public class UIAppcastUpdater : AppcastUpdater
{
	private string _Destination;

	private bool _UserChooseDestination;

	private string _ExtraCommandLine;

	private string _ApplicationName;

	private Image _Image;

	private string _SkipBuild;

	private string _CurrentVersion;

	private DownloadCompletePrompt _DownloadCompletePrompt = DownloadCompletePrompt.InstallAndRelaunch;

	private DownloadStatus DownloadStatus;

	private EventHandler<AppcastUpdaterDownloadEventArgs> InstallHandler;

	private FormClosedEventHandler DownloadStatusClosed;

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

	public bool UserChooseDestination
	{
		get
		{
			return _UserChooseDestination;
		}
		set
		{
			_UserChooseDestination = value;
		}
	}

	public string ExtraCommandLine
	{
		get
		{
			return _ExtraCommandLine;
		}
		set
		{
			_ExtraCommandLine = value;
		}
	}

	public string ApplicationName
	{
		get
		{
			return _ApplicationName;
		}
		set
		{
			_ApplicationName = value;
		}
	}

	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			_Image = value;
		}
	}

	public string SkipBuild
	{
		get
		{
			return _SkipBuild;
		}
		set
		{
			_SkipBuild = value;
		}
	}

	public string CurrentVersion
	{
		get
		{
			return _CurrentVersion;
		}
		set
		{
			_CurrentVersion = value;
		}
	}

	public DownloadCompletePrompt DownloadCompletePrompt
	{
		get
		{
			return _DownloadCompletePrompt;
		}
		set
		{
			_DownloadCompletePrompt = value;
			if (DownloadStatus != null)
			{
				DownloadStatus.DownloadCompletePrompt = value;
			}
		}
	}

	public UIAppcastUpdater(string currentBuild)
		: base(currentBuild)
	{
		Initialize();
	}

	public UIAppcastUpdater(string currentBuild, AppcastFeed appcastFeed)
		: base(currentBuild, appcastFeed)
	{
		Initialize();
	}

	public UIAppcastUpdater(string currentBuild, AppcastFeed appcastFeed, TimeSpan checkInterval)
		: base(currentBuild, appcastFeed, checkInterval)
	{
		Initialize();
	}

	public UIAppcastUpdater(string currentBuild, AppcastFeed appcastFeed, TimeSpan checkInterval, DateTime lastUpdateCheck)
		: base(currentBuild, appcastFeed, checkInterval, lastUpdateCheck)
	{
		Initialize();
	}

	private void Initialize()
	{
		InstallHandler = DownloadStatus_InstallClick;
		DownloadStatusClosed = DownloadStatus_FormClosed;
	}

	public void SetUserAgent()
	{
		string text = "";
		if (!string.IsNullOrEmpty(ApplicationName))
		{
			text += ApplicationName;
			if (!string.IsNullOrEmpty(CurrentVersion))
			{
				text = text + "/" + CurrentVersion;
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			text += " ";
		}
		AppcastSettings.UserAgent = text + AppcastSettings.DefaultUserAgent;
	}

	protected override AppcastItem CheckForUpdate(bool auto)
	{
		if (DownloadStatus != null && DownloadStatus.Visible)
		{
			DownloadStatus.BringToFront();
			return null;
		}
		return base.CheckForUpdate(auto);
	}

	protected override void OnUpdateAvailable(UpdateAvailableEventArgs e)
	{
		if (!e.IsAutoCheck || string.Compare(e.Item.Build, SkipBuild, ignoreCase: true) > 0)
		{
			PromptUserUpdateAction(e);
		}
		base.OnUpdateAvailable(e);
		if (e.Download)
		{
			DownloadStatus = new DownloadStatus(this, ApplicationName, Image);
			DownloadStatus.DownloadCompletePrompt = DownloadCompletePrompt;
			DownloadStatus.InstallClick += InstallHandler;
			DownloadStatus.InstallAndRelaunchClick += InstallHandler;
			DownloadStatus.FormClosed += DownloadStatusClosed;
		}
	}

	protected override void OnCheckedForUpdate(CheckedForUpdateEventArgs e)
	{
		base.OnCheckedForUpdate(e);
		if (!e.UpdateFound && !e.IsAutoCheck)
		{
			if (!base.AppcastFeed.Found)
			{
				MessageBox.Show("Could not retrieve update information. Please make sure you are connected to the internet.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (string.IsNullOrEmpty(ApplicationName))
			{
				MessageBox.Show("No update was found.", "No Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				MessageBox.Show("No " + _ApplicationName + " update was found.", "No Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
	}

	private void PromptUserUpdateAction(UpdateAvailableEventArgs e)
	{
		UpdateDialog updateDialog = new UpdateDialog(ApplicationName, e.Item, Image);
		updateDialog.CurrentVersion = CurrentVersion;
		updateDialog.CurrentBuild = base.CurrentBuild;
		UpdateDialogResult updateDialogResult = updateDialog.ShowDialog();
		updateDialog.Dispose();
		switch (updateDialogResult)
		{
		case UpdateDialogResult.InstallUpdate:
			if (UserChooseDestination)
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				saveFileDialog.FileName = AppcastUpdater.GetFilenameFromPath(e.Item.Location);
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					e.Download = true;
					string destination = (Destination = saveFileDialog.FileName);
					e.Destination = destination;
				}
				saveFileDialog.Dispose();
			}
			else
			{
				e.Download = true;
				e.Destination = Destination;
			}
			break;
		case UpdateDialogResult.SkipThisVersion:
			SkipBuild = e.Item.Build;
			break;
		case UpdateDialogResult.RemindMeLater:
			SkipBuild = null;
			break;
		}
	}

	private void DownloadStatus_InstallClick(object owner, AppcastUpdaterDownloadEventArgs e)
	{
		Process process = new Process();
		process.StartInfo.FileName = e.Destination;
		process.StartInfo.Arguments = ExtraCommandLine;
		process.StartInfo.UseShellExecute = true;
		try
		{
			process.Start();
		}
		catch
		{
		}
	}

	private void DownloadStatus_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (DownloadStatus != null)
		{
			DownloadStatus.InstallClick -= InstallHandler;
			DownloadStatus.InstallAndRelaunchClick -= InstallHandler;
			DownloadStatus.FormClosed -= DownloadStatusClosed;
			DownloadStatus.Dispose();
			DownloadStatus = null;
		}
	}
}
