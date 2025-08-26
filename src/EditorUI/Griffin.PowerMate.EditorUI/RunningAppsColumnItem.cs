using System;
using System.Diagnostics;
using System.Drawing;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class RunningAppsColumnItem : IColumnItem, IDisposable
{
	private bool IsSelected;

	private string AppName;

	private string AppPath;

	private Icon _Icon;

	private bool IsDisposed;

	public bool Selected
	{
		get
		{
			return IsSelected;
		}
		set
		{
			if (IsSelected != value)
			{
				IsSelected = value;
				if (this.SelectedChanged != null)
				{
					this.SelectedChanged(this);
				}
			}
		}
	}

	public Icon Icon => _Icon;

	public string Text
	{
		get
		{
			if (AppName != null)
			{
				return AppName;
			}
			return AppNode.GlobalName;
		}
		set
		{
			if (AppName != value)
			{
				AppName = value;
				if (this.TextChanged != null)
				{
					this.TextChanged(this);
				}
			}
		}
	}

	public Color TextColor => Color.Transparent;

	public string Image => AppName;

	public string Path => AppPath;

	public event EventHandler Disposed;

	public event ColumnItemHandler TextChanged;

	public event ColumnItemHandler IconChanged;

	public event ColumnItemHandler SelectedChanged;

	public RunningAppsColumnItem(Process process)
	{
		if (process != null)
		{
			AppName = process.ProcessName;
			try
			{
				AppPath = process.MainModule.FileName;
				_Icon = IconHelper.GetIconFromFile(AppPath, 0, IconSize.Small);
			}
			catch
			{
			}
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

	protected virtual void OnIconChanged()
	{
		if (this.IconChanged != null)
		{
			this.IconChanged(this);
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
