using Griffin.PowerMate.PowerMate;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class PMNotifyIconUI : IPowerMateUIPlugin, IDisposable
{
	protected class NotifyIconSettingTypes
	{
		public const string ShowInTaskbar = "showTaskbarIcon";
	}

	private UIStatus CurrentStatus = UIStatus.Closed;

	private CustomNotifyIcon PMNotifyIcon;

	private ContextMenuStrip PowerMateMenu;

	private RestartManagerMessageFilter RMMessageFilter;

	private PowerMateDoc _PowerMateDoc;

	private bool IsDisposed;

	[CompilerGenerated]
	private static EventHandler<MessageEventArgs> _003C_003E9__CachedAnonymousMethodDelegate1;

	public string Name => "PowerMate 2.0 Notification Icon and Menu";

	public string Description => "Displays a notification icon with a menu to select a UI or quit the PowerMate application.";

	public string Author => "Griffin Technology";

	public string Version => "1.0";

	public UIStatus Status => CurrentStatus;

	public PowerMateDoc PowerMateDoc
	{
		set
		{
			_PowerMateDoc = value;
		}
	}

	public bool ShowInTaskbar
	{
		get
		{
			try
			{
				return bool.Parse(_PowerMateDoc.GetSetting("showTaskbarIcon").ToLower());
			}
			catch
			{
				return true;
			}
		}
		set
		{
			if (_PowerMateDoc != null)
			{
				_PowerMateDoc.SetSetting("showTaskbarIcon", value.ToString());
			}
			if (value)
			{
				Open(_PowerMateDoc);
			}
			else
			{
				Close();
			}
		}
	}

	public event EventHandler StatusChanged;

	protected event EventHandler NotifyIconDoubleClicked;

	public PMNotifyIconUI()
	{
		PowerMateApp.UIPluginsLoaded += PowerMateApp_UIPluginsLoaded;
		ActivateFilter activateFilter = new ActivateFilter();
		activateFilter.Activated += activateFilter_Activated;
		Application.AddMessageFilter(activateFilter);
		_PowerMateDoc = PowerMateApp.PowerMateDoc;
	}

	public void Open(PowerMateDoc powerMateDoc)
	{
		if (CurrentStatus != UIStatus.Open)
		{
			_PowerMateDoc = powerMateDoc;
			PMNotifyIcon.Visible = true;
			CurrentStatus = UIStatus.Open;
			OnStatusChanged(EventArgs.Empty);
		}
	}

	public void Close()
	{
		if (CurrentStatus == UIStatus.Open)
		{
			PMNotifyIcon.Visible = false;
			CurrentStatus = UIStatus.Closed;
			OnStatusChanged(EventArgs.Empty);
		}
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			while (PowerMateMenu.Items.Count > 0)
			{
				PowerMateMenu.Items[0].Dispose();
			}
			PowerMateMenu.Dispose();
			this.NotifyIconDoubleClicked = null;
			PMNotifyIcon.Icon.Dispose();
			PMNotifyIcon.Dispose();
			CurrentStatus = UIStatus.Closed;
			OnStatusChanged(EventArgs.Empty);
		}
	}

	protected virtual void OnStatusChanged(EventArgs e)
	{
		if (this.StatusChanged != null)
		{
			this.StatusChanged(this, e);
		}
	}

	protected virtual void OnNotifyIconDoubleClicked(EventArgs e)
	{
		if (this.NotifyIconDoubleClicked != null)
		{
			this.NotifyIconDoubleClicked(this, e);
		}
	}

	private void InitializeNotifyIcon()
	{
		PowerMateMenu = new ContextMenuStrip();
		PMNotifyIcon = new CustomNotifyIcon();
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Quit PowerMate");
		ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Hide Taskbar Icon");
		toolStripMenuItem.Click += quitPowerMateMenuItem_Click;
		toolStripMenuItem2.Click += hidePowerMateMenuItem_Click;
		PowerMateMenu.Items.AddRange(new ToolStripItem[2] { toolStripMenuItem, toolStripMenuItem2 });
		if (PowerMateApp.UIPlugins.Length > 1)
		{
			PowerMateMenu.Items.Add("-");
			IPowerMateUIPlugin[] uIPlugins = PowerMateApp.UIPlugins;
			foreach (IPowerMateUIPlugin powerMateUIPlugin in uIPlugins)
			{
				if (powerMateUIPlugin != this)
				{
					PMNotifyMenuItem pMNotifyMenuItem = new PMNotifyMenuItem(powerMateUIPlugin);
					pMNotifyMenuItem.Click += menuItem_Click;
					PowerMateMenu.Items.Add(pMNotifyMenuItem);
					if (powerMateUIPlugin is INotifyIconDoubleClick)
					{
						this.NotifyIconDoubleClicked = (EventHandler)Delegate.Combine(this.NotifyIconDoubleClicked, ((INotifyIconDoubleClick)powerMateUIPlugin).NotifyIconDoubleClicked);
					}
				}
			}
		}
		PMNotifyIcon.ContextMenuStrip = PowerMateMenu;
		RMMessageFilter = new RestartManagerMessageFilter();
		RMMessageFilter.QueryEndSession += delegate
		{
			Application.Exit();
		};
		PMNotifyIcon.AddMessageFilter(RMMessageFilter);
		PMNotifyIcon.Icon = Resources.PowerMate;
		PMNotifyIcon.Text = "PowerMate 2.0";
		PMNotifyIcon.DoubleClick += PMNotifyIcon_DoubleClick;
	}

	private void PMNotifyIcon_DoubleClick(object sender, EventArgs e)
	{
		OnNotifyIconDoubleClicked(e);
	}

	private void menuItem_Click(object sender, EventArgs e)
	{
		((PMNotifyMenuItem)sender).Plugin.Open(_PowerMateDoc);
	}

	private void quitPowerMateMenuItem_Click(object sender, EventArgs e)
	{
		Application.Exit();
	}

	private void hidePowerMateMenuItem_Click(object sender, EventArgs e)
	{
		ShowInTaskbar = false;
	}

	private void activateFilter_Activated(object sender, EventArgs e)
	{
		ShowInTaskbar = true;
		Open(_PowerMateDoc);
	}

	private void PowerMateApp_UIPluginsLoaded(object sender, EventArgs e)
	{
		InitializeNotifyIcon();
		if (ShowInTaskbar)
		{
			Open(PowerMateApp.PowerMateDoc);
		}
	}
}
