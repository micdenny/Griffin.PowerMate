using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Griffin.PowerMate.EditorUI;

internal class Column : UserControl
{
	public delegate void ColumnHandler(object sender, IColumnItem item);

	private ColumnType TypeOfColumn = ColumnType.IconAndText;

	private int HeightOfRow = 24;

	private bool SelectMultiple = true;

	private bool AltRowColor = true;

	private Color AlternateBackColor = Color.AliceBlue;

	private Color SelectColor = SystemColors.MenuHighlight;

	private bool HideSelect;

	private Color DimSelectColor = SystemColors.MenuBar;

	private Color SelectTextColor = Color.Transparent;

	private bool RowLinesDraw;

	private Pen RowLinesPen = new Pen(SystemColors.InactiveBorder, 1f);

	private ContentAlignment AlignmentOfItems = ContentAlignment.MiddleLeft;

	private int RowSpace = 3;

	private int LeftEdge = 3;

	private int RightEdge = 1;

	private int IconTextPixels = 3;

	private bool TextEditable;

	private bool DragIsAllowed = true;

	private StringFormat TextFormat = new StringFormat(StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap);

	private Pen CurrentSelectedPen = new Pen(Color.FromArgb(128, Color.Black), 1f);

	private bool DrawSelectionOutline = true;

	private Point PaintOffset = new Point(0, 0);

	private ColumnItemCollection ColumnItems = new ColumnItemCollection();

	private int SelectedFromIndex = -1;

	private int myCurrentIndex = -1;

	private bool InSelectionProcess;

	private string TempPrevItemText;

	private IColumnItem DragItem;

	private Rectangle DragRect = Rectangle.Empty;

	private IContainer components;

	private TextBox ChangeText;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColumnType Type
	{
		get
		{
			return TypeOfColumn;
		}
		set
		{
			TypeOfColumn = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int RowHeight
	{
		get
		{
			return HeightOfRow;
		}
		set
		{
			HeightOfRow = value;
			Invalidate();
			if (this.TotalLengthChanged != null)
			{
				this.TotalLengthChanged(this, new EventArgs());
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MultiSelect
	{
		get
		{
			return SelectMultiple;
		}
		set
		{
			if (!value && myCurrentIndex >= 0)
			{
				SelectOne(myCurrentIndex);
			}
			SelectMultiple = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AlternateRowColor
	{
		get
		{
			return AltRowColor;
		}
		set
		{
			AltRowColor = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color AltBackColor
	{
		get
		{
			return AlternateBackColor;
		}
		set
		{
			AlternateBackColor = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectRowColor
	{
		get
		{
			return SelectColor;
		}
		set
		{
			SelectColor = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool HideSelection
	{
		get
		{
			return HideSelect;
		}
		set
		{
			HideSelect = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color HideSelectRowColor
	{
		get
		{
			return DimSelectColor;
		}
		set
		{
			DimSelectColor = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectForeColor
	{
		get
		{
			return SelectTextColor;
		}
		set
		{
			SelectTextColor = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool DrawRowLines
	{
		get
		{
			return RowLinesDraw;
		}
		set
		{
			RowLinesDraw = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color RowLinesColor
	{
		get
		{
			return RowLinesPen.Color;
		}
		set
		{
			RowLinesPen.Color = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ContentAlignment ItemAlignment
	{
		get
		{
			return AlignmentOfItems;
		}
		set
		{
			AlignmentOfItems = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int RowSpacing
	{
		get
		{
			return RowSpace;
		}
		set
		{
			RowSpace = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Indent
	{
		get
		{
			return LeftEdge;
		}
		set
		{
			LeftEdge = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int IconTextSpace
	{
		get
		{
			return IconTextPixels;
		}
		set
		{
			IconTextPixels = value;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool SelectionOutline
	{
		get
		{
			return DrawSelectionOutline;
		}
		set
		{
			if (value != DrawSelectionOutline)
			{
				DrawSelectionOutline = value;
				Invalidate();
			}
		}
	}

	public ColumnItemCollection Items => ColumnItems;

	public ColumnItemCollection SelectedItems => ColumnItems.SelectedItems;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point Offset
	{
		get
		{
			return PaintOffset;
		}
		set
		{
			if (ChangeText.Visible)
			{
				ChangeTextDone(cancel: false);
			}
			PaintOffset = value;
			Invalidate();
		}
	}

	public int TotalLength => ColumnItems.Count * HeightOfRow;

	public int CurrentIndex => myCurrentIndex;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool TextEdit
	{
		get
		{
			return TextEditable;
		}
		set
		{
			TextEditable = value;
			if (ChangeText.Visible)
			{
				ChangeTextDone(cancel: true);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowDrag
	{
		get
		{
			return DragIsAllowed;
		}
		set
		{
			DragIsAllowed = value;
		}
	}

	public event EventHandler ItemSelectionChanged;

	public event EventHandler TotalLengthChanged;

	public event ColumnHandler UserChangeText;

	public event ColumnHandler ItemMouseClick;

	public Column()
	{
		InitializeComponent();
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		ColumnItems.ItemAdded += OnItemAdded;
		ColumnItems.ItemChanged += OnItemChanged;
		ColumnItems.ItemRemoved += OnItemRemoved;
		ColumnItems.ItemSelectedChanged += OnItemSelectedChanged;
		ColumnItems.ItemTextChanged += OnItemIconOrTextChanged;
		ColumnItems.ItemIconChanged += OnItemIconOrTextChanged;
		TextFormat.Trimming = StringTrimming.EllipsisCharacter;
		CurrentSelectedPen.DashStyle = DashStyle.Dot;
	}

	protected void OnItemRemoved(ColumnItemCollection sender, IColumnItem item, int index)
	{
		if (index == SelectedFromIndex)
		{
			SelectedFromIndex = -1;
		}
		else if (index < SelectedFromIndex)
		{
			SelectedFromIndex--;
		}
		if (index == myCurrentIndex)
		{
			myCurrentIndex = -1;
		}
		else if (index < myCurrentIndex)
		{
			myCurrentIndex--;
		}
		Invalidate();
		if (this.TotalLengthChanged != null)
		{
			this.TotalLengthChanged(this, new EventArgs());
		}
		if (item.Selected && this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, new EventArgs());
		}
	}

	protected void OnItemChanged(ColumnItemCollection sender, IColumnItem item, int index)
	{
		bool flag = EnforceMultiSelect(item, index);
		Invalidate();
		if (flag && this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, new EventArgs());
		}
	}

	protected void OnItemAdded(ColumnItemCollection sender, IColumnItem item, int index)
	{
		if (index <= SelectedFromIndex)
		{
			SelectedFromIndex++;
		}
		if (index <= myCurrentIndex)
		{
			myCurrentIndex++;
		}
		bool flag = EnforceMultiSelect(item, index);
		Invalidate();
		if (this.TotalLengthChanged != null)
		{
			this.TotalLengthChanged(this, new EventArgs());
		}
		if (flag && this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, new EventArgs());
		}
	}

	protected void OnItemSelectedChanged(ColumnItemCollection sender, IColumnItem item, int index)
	{
		if (InSelectionProcess)
		{
			InSelectionProcess = false;
			return;
		}
		EnforceMultiSelect(item, index);
		Invalidate();
		if (this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, new EventArgs());
		}
	}

	protected void OnItemIconOrTextChanged(ColumnItemCollection sender, IColumnItem item, int index)
	{
		Invalidate();
	}

	private bool EnforceMultiSelect(IColumnItem item, int index)
	{
		bool result = false;
		if (item.Selected)
		{
			if (!SelectMultiple && index != myCurrentIndex && myCurrentIndex >= 0 && ColumnItems[myCurrentIndex].Selected)
			{
				InSelectionProcess = true;
				ColumnItems[myCurrentIndex].Selected = false;
				result = true;
			}
			SelectedFromIndex = (myCurrentIndex = index);
		}
		return result;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		Graphics graphics = e.Graphics;
		graphics.TranslateTransform(-PaintOffset.X, -PaintOffset.Y);
		DrawBGRows(graphics);
		for (int i = 0; i < ColumnItems.Count; i++)
		{
			if (CheckDrawIcon(i))
			{
				Image image = ColumnItems[i].Icon.ToBitmap();
				graphics.DrawImage(image, GetIconRectangle(graphics, i));
				image.Dispose();
			}
			if (CheckDrawText(i))
			{
				Brush brush = DetermineProperFontBrush(ColumnItems[i]);
				graphics.DrawString(ColumnItems[i].Text, Font, brush, GetTextRectangle(graphics, i), TextFormat);
				brush.Dispose();
			}
			if ((Focused || ChangeText.Focused) && i == myCurrentIndex && DrawSelectionOutline)
			{
				if (RowLinesDraw)
				{
					graphics.DrawRectangle(CurrentSelectedPen, base.ClientRectangle.Left, i * HeightOfRow, base.ClientRectangle.Width - 1, HeightOfRow - 2);
				}
				else
				{
					graphics.DrawRectangle(CurrentSelectedPen, base.ClientRectangle.Left, i * HeightOfRow, base.ClientRectangle.Width - 1, HeightOfRow - 1);
				}
			}
		}
	}

	protected void DrawBGRows(Graphics control)
	{
		SolidBrush solidBrush = new SolidBrush(SelectColor);
		SolidBrush solidBrush2 = new SolidBrush(DimSelectColor);
		SolidBrush solidBrush3 = new SolidBrush(AlternateBackColor);
		bool flag = false;
		for (int i = 0; i < ColumnItems.Count || i <= base.Height / HeightOfRow; i++)
		{
			if (i < ColumnItems.Count && ColumnItems[i].Selected)
			{
				if (Focused || ChangeText.Focused)
				{
					control.FillRectangle(solidBrush, 0, i * HeightOfRow, base.Width, HeightOfRow);
				}
				else if (!HideSelect)
				{
					control.FillRectangle(solidBrush2, 0, i * HeightOfRow, base.Width, HeightOfRow);
				}
			}
			else if (flag)
			{
				control.FillRectangle(solidBrush3, 0, i * HeightOfRow, base.Width, HeightOfRow);
			}
			if (RowLinesDraw)
			{
				control.DrawLine(RowLinesPen, 0, i * HeightOfRow - 1, base.Width, i * HeightOfRow - 1);
			}
			if (AltRowColor)
			{
				flag = !flag;
			}
		}
		solidBrush.Dispose();
		solidBrush2.Dispose();
		solidBrush3.Dispose();
	}

	protected bool CheckDrawIcon(int index)
	{
		bool result = false;
		if (ColumnItems[index].Icon != null)
		{
			if (TypeOfColumn < ColumnType.TextOrIcon)
			{
				result = true;
			}
			else if (TypeOfColumn == ColumnType.TextOrIcon && (ColumnItems[index].Text == null || ColumnItems[index].Text == ""))
			{
				result = true;
			}
		}
		return result;
	}

	protected bool CheckDrawText(int index)
	{
		bool result = false;
		if (ColumnItems[index].Text != null)
		{
			if (TypeOfColumn > ColumnType.IconOrText)
			{
				result = true;
			}
			else if (TypeOfColumn == ColumnType.IconOrText && ColumnItems[index].Icon == null)
			{
				result = true;
			}
		}
		return result;
	}

	protected Rectangle GetIconRectangle(Graphics graphic, int index)
	{
		int num = HeightOfRow - RowSpace;
		Size size = new Size(num, num);
		if (index < ColumnItems.Count && ColumnItems[index].Icon != null)
		{
			if (ColumnItems[index].Icon.Width < num)
			{
				size.Width = ColumnItems[index].Icon.Width;
			}
			if (ColumnItems[index].Icon.Height < num)
			{
				size.Height = ColumnItems[index].Icon.Height;
			}
		}
		Rectangle bounds = new Rectangle(LeftEdge, index * HeightOfRow + RowSpace / 2, base.ClientSize.Width - (LeftEdge + RightEdge), num);
		if (CheckDrawText(index))
		{
			bounds.Width -= (int)graphic.MeasureString(ColumnItems[index].Text, Font, bounds.Width - (num + IconTextPixels), TextFormat).Width + IconTextPixels + 1;
		}
		return new Rectangle(FindAlignmentPoint(new Size(num, num), AlignmentOfItems, bounds), size);
	}

	protected Rectangle GetTextRectangle(Graphics graphic, int index)
	{
		int num = HeightOfRow - RowSpace;
		Rectangle bounds = new Rectangle(LeftEdge, index * HeightOfRow + RowSpace / 2, base.ClientSize.Width - (LeftEdge + RightEdge), num);
		if (CheckDrawIcon(index) || TypeOfColumn == ColumnType.IconAndIndentText)
		{
			bounds.Width -= num + IconTextPixels;
			bounds.X += num + IconTextPixels;
		}
		Size size = Size.Round(graphic.MeasureString(ColumnItems[index].Text, Font, bounds.Width, TextFormat));
		size.Width++;
		return new Rectangle(FindAlignmentPoint(size, AlignmentOfItems, bounds), size);
	}

	private static Point FindAlignmentPoint(Size toAlign, ContentAlignment alignment, Rectangle bounds)
	{
		Point location = bounds.Location;
		if (alignment.ToString().Contains("Center"))
		{
			location.X += (bounds.Width - toAlign.Width) / 2;
		}
		else if (alignment.ToString().Contains("Right"))
		{
			location.X = bounds.Right - toAlign.Width;
		}
		if (alignment.ToString().Contains("Middle"))
		{
			location.Y += (bounds.Height - toAlign.Height) / 2;
		}
		else if (alignment.ToString().Contains("Bottom"))
		{
			location.Y += bounds.Height - toAlign.Height;
		}
		return location;
	}

	protected Brush DetermineProperFontBrush(IColumnItem item)
	{
		if (SelectTextColor.A != 0 && item.Selected && (Focused || ChangeText.Focused))
		{
			return new SolidBrush(SelectTextColor);
		}
		if (item.TextColor.A != 0)
		{
			return new SolidBrush(item.TextColor);
		}
		return new SolidBrush(ForeColor);
	}

	private void Column_Resize(object sender, EventArgs e)
	{
		Invalidate();
	}

	public void SelectOne(int index)
	{
		bool flag = false;
		if (SelectMultiple && myCurrentIndex >= 0)
		{
			for (int i = 0; i < ColumnItems.Count; i++)
			{
				if (i != index && ColumnItems[i].Selected)
				{
					InSelectionProcess = true;
					ColumnItems[i].Selected = false;
					flag = true;
				}
			}
		}
		else if (!SelectMultiple && myCurrentIndex >= 0 && myCurrentIndex != index && ColumnItems[myCurrentIndex].Selected)
		{
			InSelectionProcess = true;
			ColumnItems[myCurrentIndex].Selected = false;
			flag = true;
		}
		if (!ColumnItems[index].Selected)
		{
			InSelectionProcess = true;
			ColumnItems[index].Selected = true;
			flag = true;
		}
		if (flag)
		{
			SelectedFromIndex = (myCurrentIndex = index);
			Invalidate();
			if (this.ItemSelectionChanged != null)
			{
				this.ItemSelectionChanged(this, new EventArgs());
			}
		}
	}

	public void SelectTo(int index)
	{
		if (MultiSelect)
		{
			bool flag = false;
			int num = Math.Min(index, SelectedFromIndex);
			int num2 = Math.Max(index, SelectedFromIndex);
			for (int i = 0; i < ColumnItems.Count; i++)
			{
				if (i < num || i > num2)
				{
					if (ColumnItems[i].Selected)
					{
						InSelectionProcess = true;
						ColumnItems[i].Selected = false;
						flag = true;
					}
				}
				else if (!ColumnItems[i].Selected)
				{
					InSelectionProcess = true;
					ColumnItems[i].Selected = true;
					flag = true;
				}
			}
			if (flag)
			{
				myCurrentIndex = index;
				Invalidate();
				if (this.ItemSelectionChanged != null)
				{
					this.ItemSelectionChanged(this, new EventArgs());
				}
			}
		}
		else
		{
			SelectOne(index);
		}
	}

	public void SelectChange(int index)
	{
		if (!MultiSelect && myCurrentIndex >= 0 && myCurrentIndex != index && ColumnItems[myCurrentIndex].Selected)
		{
			InSelectionProcess = true;
			ColumnItems[myCurrentIndex].Selected = false;
		}
		SelectedFromIndex = (myCurrentIndex = index);
		ColumnItems[index].Selected = !ColumnItems[index].Selected;
	}

	public void SelectAdd(int index)
	{
		if (MultiSelect)
		{
			SelectedFromIndex = (myCurrentIndex = index);
			if (!ColumnItems[index].Selected)
			{
				ColumnItems[index].Selected = true;
			}
			else
			{
				Invalidate();
			}
		}
		else
		{
			SelectOne(index);
		}
	}

	public void UnSelectAll()
	{
		if (myCurrentIndex < 0)
		{
			return;
		}
		InSelectionProcess = true;
		if (MultiSelect)
		{
			foreach (ColumnItem columnItem in ColumnItems)
			{
				if (columnItem.Selected)
				{
					columnItem.Selected = false;
				}
			}
		}
		else
		{
			ColumnItems[myCurrentIndex].Selected = false;
		}
		SelectedFromIndex = (myCurrentIndex = -1);
		Invalidate();
		if (this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, new EventArgs());
		}
	}

	private void MouseSelect(int index, MouseButtons buttons)
	{
		if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
		{
			SelectTo(index);
			return;
		}
		if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
		{
			SelectChange(index);
			return;
		}
		bool selected = ColumnItems[index].Selected;
		if (!selected || buttons != MouseButtons.Right)
		{
			SelectOne(index);
		}
		if (selected && TextEditable)
		{
			ChangeItemText(index);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (!Focused)
		{
			Focus();
		}
		if (ChangeText.Visible)
		{
			ChangeTextDone(cancel: false);
		}
		int num = HitTestIndex(e.Location);
		if (num >= 0)
		{
			if (this.ItemMouseClick != null)
			{
				this.ItemMouseClick(this, ColumnItems[num]);
			}
			MouseSelect(num, e.Button);
		}
		base.OnMouseDown(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		base.OnLostFocus(e);
		Invalidate();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		Invalidate();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (msg.Msg == 256 || msg.Msg == 260)
		{
			int num = 0;
			Keys keys = keyData & Keys.KeyCode;
			if (keys == Keys.Up || keys == Keys.Down)
			{
				if (ChangeText.Visible)
				{
					ChangeTextDone(cancel: false);
				}
				num = ((keys != Keys.Up) ? (myCurrentIndex + 1) : (myCurrentIndex - 1));
				if (num >= 0 && num < ColumnItems.Count)
				{
					if ((keyData & Keys.Shift) == Keys.Shift)
					{
						SelectTo(num);
					}
					else if ((keyData & Keys.Control) == Keys.Control)
					{
						SelectAdd(num);
					}
					else
					{
						SelectOne(num);
					}
				}
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void ChangeItemText(int index)
	{
		if (index >= 0 && index < ColumnItems.Count && CheckDrawText(index))
		{
			myCurrentIndex = index;
			if (ChangeText.Text == ColumnItems[index].Text)
			{
				OnChangeTextChanged(this, new EventArgs());
			}
			else
			{
				ChangeText.Text = ColumnItems[index].Text;
			}
			TempPrevItemText = ColumnItems[index].Text;
			ChangeText.Enabled = true;
			ChangeText.Visible = true;
			ChangeText.Focus();
			Invalidate();
		}
	}

	private void OnChangeTextChanged(object sender, EventArgs e)
	{
		ColumnItems[myCurrentIndex].Text = ChangeText.Text;
		Graphics graphics = CreateGraphics();
		Rectangle textRectangle = GetTextRectangle(graphics, myCurrentIndex);
		textRectangle.Offset(-PaintOffset.X, -PaintOffset.Y);
		textRectangle.Y = FindAlignmentPoint(ChangeText.Size, AlignmentOfItems, textRectangle).Y + 1;
		ChangeText.Location = textRectangle.Location;
		if (textRectangle.Width < 15)
		{
			ChangeText.Width = 25;
		}
		else if (textRectangle.Right + 10 > base.ClientRectangle.Width - RightEdge)
		{
			ChangeText.Width = base.ClientRectangle.Width - (textRectangle.Left + RightEdge);
		}
		else
		{
			ChangeText.Width = textRectangle.Width + 10;
		}
		graphics.Dispose();
	}

	private void OnChangeTextKeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r')
		{
			e.Handled = true;
			ChangeTextDone(cancel: false);
		}
		if (e.KeyChar == '\u001b')
		{
			e.Handled = true;
			ChangeTextDone(cancel: true);
		}
	}

	private void ChangeTextDone(bool cancel)
	{
		if (!ChangeText.Visible)
		{
			return;
		}
		ChangeText.Visible = false;
		ChangeText.Enabled = false;
		if (!cancel)
		{
			ColumnItems[myCurrentIndex].Text = ChangeText.Text;
			if (this.UserChangeText != null)
			{
				this.UserChangeText(this, ColumnItems[myCurrentIndex]);
			}
		}
		else
		{
			ColumnItems[myCurrentIndex].Text = TempPrevItemText;
		}
		Focus();
	}

	private void OnChangeTextLeave(object sender, EventArgs e)
	{
		ChangeTextDone(cancel: false);
	}

	private void SetDrag(MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			DragItem = HitTestItem(e.Location);
			if (DragItem != null)
			{
				Size dragSize = SystemInformation.DragSize;
				DragRect = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
			}
			else
			{
				DragRect = Rectangle.Empty;
			}
		}
	}

	public int HitTestIndex(Point coords)
	{
		int num = (coords.Y + PaintOffset.Y) / HeightOfRow;
		if (coords.Y + PaintOffset.Y >= 0 && num < ColumnItems.Count)
		{
			return num;
		}
		return -1;
	}

	public IColumnItem HitTestItem(Point coords)
	{
		int num = HitTestIndex(coords);
		if (num >= 0)
		{
			return ColumnItems[num];
		}
		return null;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			RowLinesPen.Dispose();
			CurrentSelectedPen.Dispose();
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.ChangeText = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.ChangeText.Enabled = false;
		this.ChangeText.Location = new System.Drawing.Point(63, 212);
		this.ChangeText.Name = "ChangeText";
		this.ChangeText.Size = new System.Drawing.Size(100, 20);
		this.ChangeText.TabIndex = 0;
		this.ChangeText.Visible = false;
		this.ChangeText.Leave += new System.EventHandler(OnChangeTextLeave);
		this.ChangeText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(OnChangeTextKeyPress);
		this.ChangeText.TextChanged += new System.EventHandler(OnChangeTextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.ChangeText);
		base.Name = "Column";
		base.Size = new System.Drawing.Size(229, 248);
		base.Resize += new System.EventHandler(Column_Resize);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
