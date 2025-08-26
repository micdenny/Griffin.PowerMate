using System.Drawing;

namespace Griffin.PowerMate.EditorUI;

internal class ColumnItem : IColumnItem
{
	private Icon ItemIcon;

	private string ItemText;

	private Color ColorOfText = Color.Transparent;

	private bool IsSelected;

	public Icon Icon
	{
		get
		{
			return ItemIcon;
		}
		set
		{
			if (ItemIcon != value)
			{
				ItemIcon = value;
				OnIconChanged();
			}
		}
	}

	public string Text
	{
		get
		{
			return ItemText;
		}
		set
		{
			if (ItemText != value)
			{
				ItemText = value;
				OnTextChanged();
			}
		}
	}

	public Color TextColor
	{
		get
		{
			return ColorOfText;
		}
		set
		{
			if (ColorOfText != value)
			{
				ColorOfText = value;
				OnTextChanged();
			}
		}
	}

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

	public event ColumnItemHandler TextChanged;

	public event ColumnItemHandler IconChanged;

	public event ColumnItemHandler SelectedChanged;

	public ColumnItem()
		: this("", null)
	{
	}

	public ColumnItem(string text)
		: this(text, null)
	{
	}

	public ColumnItem(string text, Color textColor)
		: this(text, textColor, null)
	{
	}

	public ColumnItem(Icon icon)
		: this("", icon)
	{
	}

	public ColumnItem(string text, Icon icon)
	{
		ItemText = text;
		ItemIcon = icon;
	}

	public ColumnItem(string text, Color textColor, Icon icon)
		: this(text, icon)
	{
		ColorOfText = textColor;
	}

	protected void OnTextChanged()
	{
		if (this.TextChanged != null)
		{
			this.TextChanged(this);
		}
	}

	protected void OnIconChanged()
	{
		if (this.IconChanged != null)
		{
			this.IconChanged(this);
		}
	}

	protected void OnSelectedChanged()
	{
		if (this.SelectedChanged != null)
		{
			this.SelectedChanged(this);
		}
	}
}
