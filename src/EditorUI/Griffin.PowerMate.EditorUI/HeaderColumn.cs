using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Griffin.PowerMate.EditorUI;

internal class HeaderColumn : UserControl
{
	private bool BorderDraw = true;

	private Pen BorderPen = new Pen(SystemColors.HotTrack, 2f);

	private IContainer components;

	private Panel HeaderPanel;

	private Label HeaderLabel;

	private VScrollBar ItemScroll;

	private Column ItemPanel;

	[Browsable(true)]
	public ColumnType Type
	{
		get
		{
			return ItemPanel.Type;
		}
		set
		{
			ItemPanel.Type = value;
		}
	}

	public ContentAlignment ItemAlignment
	{
		get
		{
			return ItemPanel.ItemAlignment;
		}
		set
		{
			ItemPanel.ItemAlignment = value;
		}
	}

	public bool DrawBorder
	{
		get
		{
			return BorderDraw;
		}
		set
		{
			if (BorderDraw != value)
			{
				BorderDraw = value;
				AdjustPanels();
				Invalidate();
			}
		}
	}

	public Color BorderColor
	{
		get
		{
			return BorderPen.Color;
		}
		set
		{
			BorderPen.Color = value;
			Invalidate();
		}
	}

	public int BorderThickness
	{
		get
		{
			return (int)(BorderPen.Width / 2f);
		}
		set
		{
			BorderPen.Width = value * 2;
			AdjustPanels();
			Invalidate();
		}
	}

	public Bitmap HeaderImage
	{
		get
		{
			return (Bitmap)HeaderPanel.BackgroundImage;
		}
		set
		{
			HeaderPanel.BackgroundImage = value;
			HeaderPanel.Height = value.Height;
			AdjustPanels();
			Invalidate();
		}
	}

	public ImageLayout HeaderImageLayout
	{
		get
		{
			return HeaderPanel.BackgroundImageLayout;
		}
		set
		{
			HeaderPanel.BackgroundImageLayout = value;
		}
	}

	public string HeaderText
	{
		get
		{
			return HeaderLabel.Text;
		}
		set
		{
			HeaderLabel.Text = value;
		}
	}

	public ContentAlignment HeaderAlignment
	{
		get
		{
			return HeaderLabel.TextAlign;
		}
		set
		{
			HeaderLabel.TextAlign = value;
		}
	}

	public Font HeaderFont
	{
		get
		{
			return HeaderLabel.Font;
		}
		set
		{
			HeaderLabel.Font = value;
		}
	}

	public Color HeaderColor
	{
		get
		{
			return HeaderLabel.ForeColor;
		}
		set
		{
			HeaderLabel.ForeColor = value;
		}
	}

	public int RowHeight
	{
		get
		{
			return ItemPanel.RowHeight;
		}
		set
		{
			ItemPanel.RowHeight = value;
		}
	}

	public int RowSpacing
	{
		get
		{
			return ItemPanel.RowSpacing;
		}
		set
		{
			ItemPanel.RowSpacing = value;
		}
	}

	public int Indent
	{
		get
		{
			return ItemPanel.Indent;
		}
		set
		{
			ItemPanel.Indent = value;
		}
	}

	public int IconTextSpace
	{
		get
		{
			return ItemPanel.IconTextSpace;
		}
		set
		{
			ItemPanel.IconTextSpace = value;
		}
	}

	public bool MultiSelect
	{
		get
		{
			return ItemPanel.MultiSelect;
		}
		set
		{
			ItemPanel.MultiSelect = value;
		}
	}

	public bool AlternateRowColor
	{
		get
		{
			return ItemPanel.AlternateRowColor;
		}
		set
		{
			ItemPanel.AlternateRowColor = value;
		}
	}

	public Color AltBackColor
	{
		get
		{
			return ItemPanel.AltBackColor;
		}
		set
		{
			ItemPanel.AltBackColor = value;
		}
	}

	public Color SelectRowColor
	{
		get
		{
			return ItemPanel.SelectRowColor;
		}
		set
		{
			ItemPanel.SelectRowColor = value;
		}
	}

	public bool HideSelection
	{
		get
		{
			return ItemPanel.HideSelection;
		}
		set
		{
			ItemPanel.HideSelection = value;
		}
	}

	public Color HideSelectRowColor
	{
		get
		{
			return ItemPanel.HideSelectRowColor;
		}
		set
		{
			ItemPanel.HideSelectRowColor = value;
		}
	}

	public Color SelectForeColor
	{
		get
		{
			return ItemPanel.SelectForeColor;
		}
		set
		{
			ItemPanel.SelectForeColor = value;
		}
	}

	public bool DrawRowLines
	{
		get
		{
			return ItemPanel.DrawRowLines;
		}
		set
		{
			ItemPanel.DrawRowLines = value;
		}
	}

	public Color RowLinesColor
	{
		get
		{
			return ItemPanel.RowLinesColor;
		}
		set
		{
			ItemPanel.RowLinesColor = value;
		}
	}

	public bool TextEdit
	{
		get
		{
			return ItemPanel.TextEdit;
		}
		set
		{
			ItemPanel.TextEdit = value;
		}
	}

	public bool SelectionOutline
	{
		get
		{
			return ItemPanel.SelectionOutline;
		}
		set
		{
			ItemPanel.SelectionOutline = value;
		}
	}

	public ColumnItemCollection Items => ItemPanel.Items;

	public ColumnItemCollection SelectedItems => ItemPanel.SelectedItems;

	public int CurrentIndex => ItemPanel.CurrentIndex;

	public override bool Focused
	{
		get
		{
			if (!base.Focused)
			{
				return ItemPanel.Focused;
			}
			return true;
		}
	}

	[Browsable(true)]
	public event EventHandler ItemSelectionChanged;

	public event Column.ColumnHandler UserChangeText;

	public event Column.ColumnHandler ItemMouseClick;

	public HeaderColumn()
	{
		InitializeComponent();
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		ItemPanel.ItemMouseClick += ItemPanel_ItemMouseClick;
		ItemScroll.LargeChange = ItemPanel.ClientSize.Height;
		UpdateScroll();
		ItemPanel.ItemSelectionChanged += OnItemSelectionChanged;
		ItemPanel.UserChangeText += OnUserChangeText;
		ItemPanel.Items.ItemRemoved += Items_ItemRemoved;
	}

	private void Items_ItemRemoved(ColumnItemCollection sender, IColumnItem item, int index)
	{
	}

	private void ItemPanel_ItemMouseClick(object sender, IColumnItem item)
	{
		OnItemMouseClick(item);
	}

	protected virtual void OnItemMouseClick(IColumnItem item)
	{
		if (this.ItemMouseClick != null)
		{
			this.ItemMouseClick(this, item);
		}
	}

	private void AdjustPanels()
	{
		int num = 0;
		if (BorderDraw)
		{
			num = BorderThickness;
		}
		HeaderPanel.Location = new Point(num, num);
		Panel headerPanel = HeaderPanel;
		int num2 = (ItemPanel.Width = base.ClientSize.Width - num * 2);
		headerPanel.Width = num2;
		ItemPanel.Location = new Point(num, HeaderPanel.Bottom);
		ItemScroll.Location = new Point(base.ClientSize.Width - (ItemScroll.Width + num), HeaderPanel.Bottom);
		VScrollBar itemScroll = ItemScroll;
		Column itemPanel = ItemPanel;
		int num4 = (ItemScroll.LargeChange = base.ClientSize.Height - (HeaderPanel.Bottom + num));
		int num6 = (itemPanel.Height = num4);
		itemScroll.Height = num6;
		UpdateScroll();
	}

	private void UpdateScroll()
	{
		if (ItemPanel.TotalLength > ItemPanel.Height)
		{
			if (!ItemScroll.Visible)
			{
				ItemScroll.LargeChange = ItemPanel.ClientSize.Height;
				ItemScroll.Visible = true;
				ItemPanel.Width = base.Width - (ItemScroll.Width + BorderThickness * 2);
			}
		}
		else if (ItemScroll.Visible)
		{
			ItemScroll.Visible = false;
			ItemPanel.Width = base.Width - BorderThickness * 2;
			ItemPanel.Offset = new Point(0, 0);
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		ItemScroll.LargeChange = ItemPanel.ClientSize.Height;
		UpdateScroll();
		ItemScroll.Value = ItemPanel.Offset.Y;
		OnResize(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		Graphics graphics = e.Graphics;
		if (BorderDraw)
		{
			graphics.DrawRectangle(BorderPen, base.ClientRectangle);
		}
	}

	public void SelectOne(int index)
	{
		ItemPanel.SelectOne(index);
	}

	public void SelectTo(int index)
	{
		ItemPanel.SelectTo(index);
	}

	public void SelectChange(int index)
	{
		ItemPanel.SelectChange(index);
	}

	public void SelectAdd(int index)
	{
		ItemPanel.SelectAdd(index);
	}

	public void UnSelectAll()
	{
		ItemPanel.UnSelectAll();
	}

	private void OnItemScroll(object sender, ScrollEventArgs e)
	{
		ItemPanel.Offset = new Point(0, e.NewValue);
	}

	private void ItemPanel_TotalLengthChanged(object sender, EventArgs e)
	{
		ItemScroll.Maximum = ItemPanel.TotalLength;
		ItemScroll.SmallChange = ItemPanel.RowHeight;
		if (ItemPanel.TotalLength - ItemPanel.Height > 0 && ItemPanel.Offset.Y > ItemPanel.TotalLength - ItemPanel.Height)
		{
			ItemPanel.Offset = new Point(0, ItemPanel.TotalLength - ItemPanel.Height);
		}
		UpdateScroll();
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		base.OnMouseWheel(e);
		int delta = e.Delta;
		int num = ItemScroll.Maximum - ItemScroll.LargeChange;
		if (delta < 0 && ItemScroll.Value < num)
		{
			if (ItemScroll.Value + ItemScroll.SmallChange <= num)
			{
				ItemScroll.Value += ItemScroll.SmallChange;
			}
			else
			{
				ItemScroll.Value = num;
			}
			OnItemScroll(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, ItemScroll.Value));
		}
		else if (delta > 0 && ItemScroll.Value > ItemScroll.Minimum)
		{
			if (ItemScroll.Value - ItemScroll.SmallChange >= ItemScroll.Minimum)
			{
				ItemScroll.Value -= ItemScroll.SmallChange;
			}
			else
			{
				ItemScroll.Value = ItemScroll.Minimum;
			}
			OnItemScroll(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, ItemScroll.Value));
		}
	}

	private void ScrollItemIntoView(int index)
	{
		int num = index * ItemPanel.RowHeight;
		if (ItemScroll.Value > num)
		{
			ItemScroll.Value = num;
			OnItemScroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, ItemScroll.Value));
		}
		else if (ItemScroll.Value < num + ItemPanel.RowHeight - ItemPanel.ClientSize.Height)
		{
			ItemScroll.Value = num + ItemPanel.RowHeight - ItemPanel.ClientSize.Height;
			OnItemScroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, ItemScroll.Value));
		}
	}

	public void ChangeItemText(int index)
	{
		Focus();
		ItemPanel.ChangeItemText(index);
	}

	protected void OnItemSelectionChanged(object sender, EventArgs e)
	{
		if (ItemPanel.CurrentIndex >= 0)
		{
			ScrollItemIntoView(ItemPanel.CurrentIndex);
		}
		if (this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, e);
		}
	}

	protected void OnUserChangeText(object sender, IColumnItem item)
	{
		if (this.UserChangeText != null)
		{
			this.UserChangeText(this, item);
		}
	}

	public int HitTestIndex(Point coords)
	{
		coords.Offset(0, -HeaderPanel.Bottom);
		return ItemPanel.HitTestIndex(coords);
	}

	public IColumnItem HitTestItem(Point coords)
	{
		coords.Offset(0, -HeaderPanel.Bottom);
		return ItemPanel.HitTestItem(coords);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			BorderPen.Dispose();
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Griffin.PowerMate.EditorUI.HeaderColumn));
		this.HeaderPanel = new System.Windows.Forms.Panel();
		this.HeaderLabel = new System.Windows.Forms.Label();
		this.ItemScroll = new System.Windows.Forms.VScrollBar();
		this.ItemPanel = new Griffin.PowerMate.EditorUI.Column();
		this.HeaderPanel.SuspendLayout();
		base.SuspendLayout();
		this.HeaderPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
		this.HeaderPanel.BackgroundImage = (System.Drawing.Image)resources.GetObject("HeaderPanel.BackgroundImage");
		this.HeaderPanel.Controls.Add(this.HeaderLabel);
		this.HeaderPanel.Location = new System.Drawing.Point(1, 1);
		this.HeaderPanel.Name = "HeaderPanel";
		this.HeaderPanel.Size = new System.Drawing.Size(239, 21);
		this.HeaderPanel.TabIndex = 0;
		this.HeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HeaderLabel.BackColor = System.Drawing.Color.Transparent;
		this.HeaderLabel.Location = new System.Drawing.Point(1, 0);
		this.HeaderLabel.Name = "HeaderLabel";
		this.HeaderLabel.Size = new System.Drawing.Size(239, 21);
		this.HeaderLabel.TabIndex = 0;
		this.HeaderLabel.Text = "Header";
		this.HeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ItemScroll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.ItemScroll.LargeChange = 2;
		this.ItemScroll.Location = new System.Drawing.Point(223, 22);
		this.ItemScroll.Maximum = 1;
		this.ItemScroll.Name = "ItemScroll";
		this.ItemScroll.Size = new System.Drawing.Size(17, 236);
		this.ItemScroll.TabIndex = 0;
		this.ItemScroll.Visible = false;
		this.ItemScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(OnItemScroll);
		this.ItemPanel.AllowDrag = true;
		this.ItemPanel.AltBackColor = System.Drawing.Color.AliceBlue;
		this.ItemPanel.AlternateRowColor = true;
		this.ItemPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ItemPanel.BackColor = System.Drawing.Color.Transparent;
		this.ItemPanel.DrawRowLines = true;
		this.ItemPanel.HideSelection = false;
		this.ItemPanel.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.ItemPanel.IconTextSpace = 3;
		this.ItemPanel.Indent = 3;
		this.ItemPanel.ItemAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.ItemPanel.Location = new System.Drawing.Point(1, 22);
		this.ItemPanel.MultiSelect = true;
		this.ItemPanel.Name = "ItemPanel";
		this.ItemPanel.Offset = new System.Drawing.Point(0, 0);
		this.ItemPanel.RowHeight = 24;
		this.ItemPanel.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.ItemPanel.RowSpacing = 3;
		this.ItemPanel.SelectForeColor = System.Drawing.Color.Transparent;
		this.ItemPanel.SelectionOutline = true;
		this.ItemPanel.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.ItemPanel.Size = new System.Drawing.Size(239, 236);
		this.ItemPanel.TabIndex = 1;
		this.ItemPanel.TextEdit = false;
		this.ItemPanel.Type = Griffin.PowerMate.EditorUI.ColumnType.IconAndText;
		this.ItemPanel.TotalLengthChanged += new System.EventHandler(ItemPanel_TotalLengthChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.ItemScroll);
		base.Controls.Add(this.HeaderPanel);
		base.Controls.Add(this.ItemPanel);
		this.DoubleBuffered = true;
		base.Name = "HeaderColumn";
		base.Size = new System.Drawing.Size(241, 259);
		this.HeaderPanel.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
