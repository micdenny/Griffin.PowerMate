using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Griffin.PowerMate.EditorUI;

internal class ColumnItemMenuItem : ToolStripMenuItem
{
	private IColumnItem _ColumnItem;

	private Image _Image;

	private ColumnItemHandler ColumnItemTextChanged;

	private ColumnItemHandler UpdateTextColor;

	private ColumnItemHandler ColumnItemIconChanged;

	private ColumnItemHandler UpdateImage;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IColumnItem ColumnItem
	{
		get
		{
			return _ColumnItem;
		}
		set
		{
			if (_ColumnItem != null)
			{
				_ColumnItem.TextChanged -= ColumnItemTextChanged;
				_ColumnItem.IconChanged -= ColumnItemIconChanged;
			}
			if (_Image != null)
			{
				_Image.Dispose();
			}
			if (value != null)
			{
				value.TextChanged += ColumnItemTextChanged;
				value.IconChanged += ColumnItemIconChanged;
				if (value.Icon != null)
				{
					_Image = value.Icon.ToBitmap();
				}
			}
			_ColumnItem = value;
			SetTextColor(_ColumnItem);
		}
	}

	public override string Text
	{
		get
		{
			if (_ColumnItem != null)
			{
				return _ColumnItem.Text;
			}
			return base.Text;
		}
	}

	public override Image Image
	{
		get
		{
			if (_Image != null)
			{
				return _Image;
			}
			return base.Image;
		}
	}

	public ColumnItemMenuItem()
	{
		ColumnItemTextChanged = ColumnItem_TextChanged;
		UpdateTextColor = SetTextColor;
		ColumnItemIconChanged = ColumnItem_IconChanged;
		UpdateImage = SetImage;
	}

	public ColumnItemMenuItem(IColumnItem columnItem)
		: this()
	{
		ColumnItem = columnItem;
	}

	private void ColumnItem_TextChanged(IColumnItem sender)
	{
		try
		{
			base.Parent.BeginInvoke(UpdateTextColor, sender);
		}
		catch
		{
		}
	}

	private void SetTextColor(IColumnItem sender)
	{
		if (sender != null && sender.TextColor != Color.Transparent)
		{
			ForeColor = sender.TextColor;
		}
		else
		{
			ForeColor = SystemColors.ControlText;
		}
	}

	private void ColumnItem_IconChanged(IColumnItem sender)
	{
		try
		{
			base.Parent.BeginInvoke(UpdateImage, sender);
		}
		catch
		{
		}
	}

	private void SetImage(IColumnItem sender)
	{
		if (_Image != null)
		{
			_Image.Dispose();
		}
		if (sender != null && sender.Icon != null)
		{
			_Image = sender.Icon.ToBitmap();
		}
		Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _Image != null)
		{
			_Image.Dispose();
		}
		base.Dispose(disposing);
	}
}
