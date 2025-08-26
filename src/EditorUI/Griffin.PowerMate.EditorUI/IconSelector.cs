using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Griffin.PowerMate.EditorUI;

internal class IconSelector : UserControl
{
	private Pen SelectedOutlinePen = new Pen(Color.Black, 1f);

	private int _IconPadding = 3;

	private int _IconSize = 32;

	private IconList _Icons = new IconList();

	private int _SelectedIconIndex = -1;

	private int _ScrollValue;

	private IContainer components;

	private VScrollBar VerticalScrollBar;

	public int IconPadding
	{
		get
		{
			return _IconPadding;
		}
		set
		{
			if (value >= 0)
			{
				_IconPadding = value;
			}
			else
			{
				_IconPadding = 0;
			}
			UpdateVerticalScroll();
			Invalidate();
		}
	}

	public int IconSize
	{
		get
		{
			return _IconSize;
		}
		set
		{
			if (value >= 0)
			{
				_IconSize = value;
			}
			else
			{
				_IconSize = 0;
			}
			UpdateVerticalScroll();
			Invalidate();
		}
	}

	public IconList Icons => _Icons;

	public Icon SelectedIcon
	{
		get
		{
			Icon result = null;
			if (SelectedIconIndex >= 0)
			{
				result = Icons[SelectedIconIndex];
			}
			return result;
		}
		set
		{
			SelectedIconIndex = Icons.IndexOf(value);
		}
	}

	public int SelectedIconIndex
	{
		get
		{
			return _SelectedIconIndex;
		}
		set
		{
			if (value >= -1 && value < Icons.Count)
			{
				_SelectedIconIndex = value;
			}
			else
			{
				_SelectedIconIndex = -1;
			}
			Invalidate();
		}
	}

	public new Size ClientSize
	{
		get
		{
			Size clientSize = base.ClientSize;
			if (VerticalScrollBar.Visible)
			{
				clientSize.Width -= VerticalScrollBar.Width;
			}
			return clientSize;
		}
		set
		{
			if (VerticalScrollBar.Visible)
			{
				value.Width += VerticalScrollBar.Width;
			}
			base.ClientSize = value;
		}
	}

	protected int NumberOfRows
	{
		get
		{
			int num = 0;
			int numberOfColumns = NumberOfColumns;
			if (numberOfColumns != 0)
			{
				num = Icons.Count / numberOfColumns;
				if (Icons.Count % numberOfColumns > 0)
				{
					num++;
				}
			}
			return num;
		}
	}

	protected int NumberOfColumns
	{
		get
		{
			int num = ClientSize.Width / SelectionDimension;
			if (num > Icons.Count)
			{
				num = Icons.Count;
			}
			return num;
		}
	}

	protected int SelectionDimension => IconSize + 2 * IconPadding;

	private int ScrollValue
	{
		get
		{
			return _ScrollValue;
		}
		set
		{
			_ScrollValue = value;
			VerticalScrollBar.Value = value;
			Invalidate();
		}
	}

	public IconSelector()
	{
		InitializeComponent();
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		SelectedOutlinePen.DashStyle = DashStyle.Dot;
		_Icons.Cleared += IconsChanged;
		ListEventHandler<Icon> value = IconsChanged;
		_Icons.ItemInserted += value;
		_Icons.ItemRemoved += value;
		_Icons.ItemReplaced += value;
		UpdateVerticalScroll();
	}

	protected int GetIconIndexFromPoint(Point point)
	{
		point.Offset(0, ScrollValue);
		int num = point.X / SelectionDimension;
		if (num < NumberOfColumns)
		{
			num += point.Y / SelectionDimension * NumberOfColumns;
			if (num < Icons.Count)
			{
				return num;
			}
		}
		return -1;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		graphics.TranslateTransform(0f, -ScrollValue);
		Rectangle rectangle = new Rectangle(IconPadding, IconPadding, IconSize, IconSize);
		int num = 0;
		for (int i = 0; i < NumberOfRows; i++)
		{
			for (int j = 0; j < NumberOfColumns; j++)
			{
				if (num >= Icons.Count)
				{
					break;
				}
				if (num == SelectedIconIndex)
				{
					Rectangle rect = rectangle;
					rect.Inflate(IconPadding, IconPadding);
					graphics.FillRectangle(SystemBrushes.Highlight, rect);
					rect.Width--;
					rect.Height--;
					graphics.DrawRectangle(SelectedOutlinePen, rect);
				}
				Bitmap bitmap = Icons[num].ToBitmap();
				graphics.DrawImage(bitmap, rectangle);
				bitmap.Dispose();
				rectangle.X += SelectionDimension;
				num++;
			}
			rectangle.X = IconPadding;
			rectangle.Y += IconSize + 2 * IconPadding;
		}
		base.OnPaint(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		Select();
		if (base.ClientRectangle.Contains(e.Location))
		{
			int iconIndexFromPoint = GetIconIndexFromPoint(e.Location);
			if (iconIndexFromPoint >= 0)
			{
				SelectedIconIndex = iconIndexFromPoint;
			}
		}
		base.OnMouseClick(e);
	}

	protected override void OnResize(EventArgs e)
	{
		UpdateVerticalScroll();
		base.OnResize(e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		int scrollValue = ScrollValue;
		scrollValue = ((e.Delta <= 0) ? (scrollValue + VerticalScrollBar.SmallChange) : (scrollValue - VerticalScrollBar.SmallChange));
		if (scrollValue < VerticalScrollBar.Minimum)
		{
			scrollValue = VerticalScrollBar.Minimum;
		}
		if (scrollValue > VerticalScrollBar.Maximum - VerticalScrollBar.LargeChange)
		{
			scrollValue = VerticalScrollBar.Maximum - VerticalScrollBar.LargeChange;
		}
		ScrollValue = scrollValue;
		base.OnMouseWheel(e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		bool flag = false;
		if ((SelectedIconIndex >= 0 && msg.Msg == 256) || msg.Msg == 260)
		{
			int num = -1;
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Up:
				num = SelectedIconIndex - NumberOfColumns;
				flag = true;
				break;
			case Keys.Down:
				num = SelectedIconIndex + NumberOfColumns;
				flag = true;
				break;
			case Keys.Left:
				num = SelectedIconIndex - 1;
				flag = true;
				break;
			case Keys.Right:
				num = SelectedIconIndex + 1;
				flag = true;
				break;
			}
			if (num >= 0 && num < Icons.Count)
			{
				SelectedIconIndex = num;
			}
		}
		if (!flag)
		{
			flag = base.ProcessCmdKey(ref msg, keyData);
		}
		return flag;
	}

	private void IconsChanged(object sender, EventArgs e)
	{
		UpdateVerticalScroll();
		Invalidate();
	}

	private void UpdateVerticalScroll()
	{
		int num = NumberOfRows * SelectionDimension;
		VerticalScrollBar.Maximum = Math.Max(num, ClientSize.Height);
		VerticalScrollBar.SmallChange = SelectionDimension;
		VerticalScrollBar.LargeChange = ClientSize.Height;
		VerticalScrollBar.Visible = num > ClientSize.Height;
		if (ScrollValue + ClientSize.Height > VerticalScrollBar.Maximum)
		{
			ScrollValue = VerticalScrollBar.Maximum - ClientSize.Height;
		}
	}

	private void VerticalScrollBar_Scroll(object owner, ScrollEventArgs se)
	{
		ScrollValue = se.NewValue;
		base.OnScroll(se);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			SelectedOutlinePen.Dispose();
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.VerticalScrollBar = new System.Windows.Forms.VScrollBar();
		base.SuspendLayout();
		this.VerticalScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
		this.VerticalScrollBar.Location = new System.Drawing.Point(274, 0);
		this.VerticalScrollBar.Name = "VerticalScrollBar";
		this.VerticalScrollBar.Size = new System.Drawing.Size(17, 227);
		this.VerticalScrollBar.TabIndex = 0;
		this.VerticalScrollBar.Visible = false;
		this.VerticalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(VerticalScrollBar_Scroll);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		base.Controls.Add(this.VerticalScrollBar);
		base.Name = "IconSelector";
		base.Size = new System.Drawing.Size(291, 227);
		base.ResumeLayout(false);
	}
}
