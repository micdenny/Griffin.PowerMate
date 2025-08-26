using System;
using System.Drawing;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class ApplicationColumnItem : NodeColumnItem<AppNode>, IDisposable
{
	private Icon _Icon;

	private bool IsDisposed;

	public override Icon Icon => _Icon;

	public override string Text
	{
		get
		{
			return myNode.Name;
		}
		set
		{
			if (myNode.Name != value)
			{
				myNode.Name = value;
			}
		}
	}

	public override Color TextColor => Color.Transparent;

	public string IconPath
	{
		get
		{
			return base.Node.GetSetting("iconPath");
		}
		set
		{
			base.Node.SetSetting("iconPath", value);
		}
	}

	public string IconFile
	{
		get
		{
			string iconPath = IconPath;
			try
			{
				int num = iconPath.LastIndexOf(',');
				int.Parse(iconPath.Substring(num + 1));
				return iconPath.Substring(0, num);
			}
			catch
			{
				return iconPath;
			}
		}
		set
		{
			IconPath = value + "," + IconIndex;
		}
	}

	public int IconIndex
	{
		get
		{
			string iconPath = IconPath;
			try
			{
				int num = iconPath.LastIndexOf(',');
				return int.Parse(iconPath.Substring(num + 1));
			}
			catch
			{
				return 0;
			}
		}
		set
		{
			IconPath = IconFile + "," + value;
		}
	}

	public event EventHandler Disposed;

	public ApplicationColumnItem(AppNode node)
		: base(node)
	{
		if (node != null)
		{
			node.NodeSettingsChanged += node_NodeSettingsChanged;
			SetIcon();
		}
	}

	private void node_NodeSettingsChanged(object sender, PowerMateNodeEventArgs e)
	{
		if (e.SettingType == "name")
		{
			OnTextChanged();
		}
		else if (e.SettingType == "iconPath")
		{
			OnIconChanged();
		}
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			if (_Icon != null)
			{
				_Icon.Dispose();
			}
			IsDisposed = true;
			OnDisposed(EventArgs.Empty);
		}
	}

	protected override void OnIconChanged()
	{
		SetIcon();
		base.OnIconChanged();
	}

	protected virtual void OnDisposed(EventArgs e)
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, e);
		}
	}

	private void SetIcon()
	{
		if (_Icon != null)
		{
			_Icon.Dispose();
		}
		if (myNode.Image != "")
		{
			_Icon = IconHelper.GetIconFromFile(IconPath, IconSize.Large);
			if (_Icon == null)
			{
				_Icon = SystemIcons.Application;
			}
		}
		else
		{
			_Icon = Resources.global;
		}
	}
}
