using System;
using System.Drawing;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class PMActionColumnItem : IColumnItem, IDisposable
{
	private Icon _Icon;

	private CActionColumnItem _CActionMenuItem;

	private bool IsDisposed;

	private ColumnItemHandler CActionSelectedChanged;

	public CActionColumnItem CActionMenuItem
	{
		get
		{
			return _CActionMenuItem;
		}
		set
		{
			bool flag = _CActionMenuItem.Selected != value.Selected;
			_CActionMenuItem.SelectedChanged -= CActionSelectedChanged;
			_CActionMenuItem = value;
			value.SelectedChanged += CActionSelectedChanged;
			if (flag)
			{
				OnSelectedChanged();
			}
		}
	}

	public bool Selected
	{
		get
		{
			return _CActionMenuItem.Selected;
		}
		set
		{
			_CActionMenuItem.Selected = value;
		}
	}

	public Icon Icon => _Icon;

	public string Text
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Color TextColor => Color.Transparent;

	public event ColumnItemHandler TextChanged;

	public event ColumnItemHandler IconChanged;

	public event ColumnItemHandler SelectedChanged;

	public event EventHandler Disposed;

	public PMActionColumnItem(CActionColumnItem cactionItem)
	{
		_Icon = GetPMActionIcon(cactionItem.Node.Action);
		CActionSelectedChanged = CActionMenuItem_SelectedChanged;
		_CActionMenuItem = cactionItem;
		cactionItem.SelectedChanged += CActionSelectedChanged;
	}

	private void CActionMenuItem_SelectedChanged(IColumnItem sender)
	{
		OnSelectedChanged();
	}

	protected static Icon GetPMActionIcon(PMAction pmaction)
	{
		Icon result = null;
		switch (pmaction)
		{
		case PMAction.ClockwiseRotate:
			result = Resources.right;
			break;
		case PMAction.CounterClockwiseRotate:
			result = Resources.left;
			break;
		case PMAction.ClickClockwiseRotate:
			result = Resources.clickRight;
			break;
		case PMAction.ClickCounterClockwiseRotate:
			result = Resources.clickLeft;
			break;
		case PMAction.Click:
			result = Resources.click;
			break;
		case PMAction.TimedClick:
			result = Resources.longClick;
			break;
		}
		return result;
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			_Icon.Dispose();
			_CActionMenuItem.SelectedChanged -= CActionSelectedChanged;
			OnDisposed(EventArgs.Empty);
		}
	}

	protected virtual void OnTextChanged()
	{
		if (this.TextChanged != null)
		{
			this.TextChanged(this);
		}
	}

	protected virtual void OnIconChanged()
	{
		if (this.IconChanged != null)
		{
			this.IconChanged(this);
		}
	}

	protected virtual void OnSelectedChanged()
	{
		if (this.SelectedChanged != null)
		{
			this.SelectedChanged(this);
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
