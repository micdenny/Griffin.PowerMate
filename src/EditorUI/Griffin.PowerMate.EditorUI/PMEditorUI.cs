using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class PMEditorUI : IPowerMateUIPlugin, INotifyIconDoubleClick
{
	private UIStatus CurrentStatus = UIStatus.Closed;

	private PowerMateEditor Editor;

	private string DefaultSettingsLocation = Application.StartupPath + "\\default.pmsettings";

	private AppCollection DefaultAppNodes;

	private PowerMateDoc _PowerMateDoc;

	private FormClosedEventHandler EditorClosed;

	private EventHandler<NodeContainerEventArgs<DeviceNode>> DeviceNodeAdded;

	private EventHandler _NotifyIconDoubleClicked;

	public string Name => "PowerMate Editor";

	public string Description => "User Interface for editing PowerMate preferences.";

	public string Author => "Griffin Technology";

	public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

	public UIStatus Status => CurrentStatus;

	public PowerMateDoc PowerMateDoc
	{
		set
		{
			if (_PowerMateDoc != value)
			{
				if (_PowerMateDoc != null)
				{
					_PowerMateDoc.ChildNodeInserted -= DeviceNodeAdded;
				}
				_PowerMateDoc = value;
				if (value != null)
				{
					value.ChildNodeInserted += DeviceNodeAdded;
				}
				if (CurrentStatus == UIStatus.Open)
				{
					Editor.PowerMateDoc = value;
				}
			}
		}
	}

	public EventHandler NotifyIconDoubleClicked => _NotifyIconDoubleClicked;

	public event EventHandler StatusChanged;

	public PMEditorUI()
	{
		EditorClosed = Editor_FormClosed;
		DeviceNodeAdded = PowerMateDoc_ChildNodeInserted;
		EventHandler notifyIconDoubleClicked = delegate
		{
			Open(_PowerMateDoc);
		};
		_NotifyIconDoubleClicked = notifyIconDoubleClicked;
		PowerMateApp.UIPluginsLoaded += PowerMateApp_UIPluginsLoaded;
		DefaultAppNodes = new AppCollection(DefaultSettingsLocation, PowerMateApp.ActionPlugins);
		PowerMateDoc = PowerMateApp.PowerMateDoc;
	}

	public void Open(PowerMateDoc powerMateDoc)
	{
		PowerMateDoc = powerMateDoc;
		if (CurrentStatus == UIStatus.Closed)
		{
			Editor = new PowerMateEditor(powerMateDoc);
			Editor.FormClosed += EditorClosed;
			Editor.DefaultAppNodes = DefaultAppNodes;
			Editor.Show();
			CurrentStatus = UIStatus.Open;
			OnStatusChanged(EventArgs.Empty);
		}
		else
		{
			Editor.Activate();
		}
	}

	public void Close()
	{
		if (CurrentStatus == UIStatus.Open)
		{
			Editor.Close();
			Editor.Dispose();
			Editor = null;
			CurrentStatus = UIStatus.Closed;
			OnStatusChanged(EventArgs.Empty);
		}
	}

	private void Editor_FormClosed(object sender, FormClosedEventArgs e)
	{
		Editor.FormClosed -= EditorClosed;
		Editor.Dispose();
		GC.Collect();
		CurrentStatus = UIStatus.Closed;
		OnStatusChanged(EventArgs.Empty);
	}

	private void PowerMateDoc_ChildNodeInserted(object sender, NodeContainerEventArgs<DeviceNode> e)
	{
		if (e.Node.Count == 0)
		{
			AppNode appNode = DefaultAppNodes.Find("", caseSensitive: false);
			appNode = ((appNode != null) ? ((AppNode)appNode.Clone()) : new AppNode(""));
			e.Node.Add(appNode);
		}
	}

	private void PowerMateApp_UIPluginsLoaded(object sender, EventArgs e)
	{
		if (!File.Exists(_PowerMateDoc.Path))
		{
			Open(_PowerMateDoc);
		}
	}

	protected virtual void OnStatusChanged(EventArgs e)
	{
		if (this.StatusChanged != null)
		{
			this.StatusChanged(this, e);
		}
	}
}
