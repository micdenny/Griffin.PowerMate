using System.Drawing;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal abstract class NodeColumnItem<T> : IColumnItem where T : PowerMateNode
{
	protected bool IsSelected;

	protected T myNode;

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

	public T Node => myNode;

	public abstract Icon Icon { get; }

	public abstract string Text { get; set; }

	public abstract Color TextColor { get; }

	public event ColumnItemHandler TextChanged;

	public event ColumnItemHandler IconChanged;

	public event ColumnItemHandler SelectedChanged;

	public NodeColumnItem(T node)
	{
		myNode = node;
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
}
