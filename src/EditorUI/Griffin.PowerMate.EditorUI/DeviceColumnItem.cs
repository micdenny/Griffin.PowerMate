using System;
using System.Drawing;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class DeviceColumnItem : NodeColumnItem<DeviceNode>, IDisposable
{
	private Icon DeviceAttachedIcon = Resources.PowerMate;

	private Icon DeviceNotAttachedIcon = Resources.dimPowerMate;

	private bool IsDisposed;

	public override Icon Icon
	{
		get
		{
			if (myNode.HasDevice)
			{
				return DeviceAttachedIcon;
			}
			return DeviceNotAttachedIcon;
		}
	}

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

	public override Color TextColor
	{
		get
		{
			if (myNode.HasDevice)
			{
				return Color.Transparent;
			}
			return SystemColors.InactiveCaptionText;
		}
	}

	public event EventHandler Disposed;

	public DeviceColumnItem(DeviceNode node)
		: base(node)
	{
		if (node != null)
		{
			node.HasDeviceChanged += node_HasDeviceChanged;
			node.NodeSettingsChanged += node_NodeSettingsChanged;
		}
	}

	private void node_NodeSettingsChanged(object sender, PowerMateNodeEventArgs e)
	{
		if (e.SettingType == "name")
		{
			OnTextChanged();
		}
	}

	private void node_HasDeviceChanged(object sender, EventArgs e)
	{
		OnIconChanged();
		OnTextChanged();
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			DeviceAttachedIcon.Dispose();
			DeviceNotAttachedIcon.Dispose();
			IsDisposed = true;
			OnDisposed(EventArgs.Empty);
		}
	}

	protected virtual void OnDisposed(EventArgs e)
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, e);
		}
	}
}
