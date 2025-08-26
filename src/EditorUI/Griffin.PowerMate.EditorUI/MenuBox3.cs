using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Griffin.PowerMate.EditorUI;

internal class MenuBox3 : Button
{
	private int ArrowBoxWidth = 25;

	private Pen ArrowBoxPen = new Pen(Color.FromArgb(200, Color.Black), 1f);

	private Brush ArrowBrush = new SolidBrush(Color.FromArgb(200, Color.Black));

	private Image OriginalImage;

	private ContextMenuStrip DropDown = new ContextMenuStrip();

	private ToolStripMenuItem CurrentItem;

	private ToolStripItemClickedEventHandler MenuClicked;

	private ToolStripItemEventHandler MenuItemAdded;

	private ToolStripItemEventHandler MenuItemRemoved;

	public ToolStripItemCollection Items => DropDown.Items;

	public ToolStripMenuItem SelectedItem
	{
		get
		{
			return CurrentItem;
		}
		set
		{
			if (value != null && value.DropDownItems.Count == 0)
			{
				if (CurrentItem != null)
				{
					CurrentItem.Checked = false;
				}
				CurrentItem = value;
				value.Checked = true;
				Image = value.Image;
				Text = value.Text;
				if (this.ItemSelected != null)
				{
					this.ItemSelected(this, new ToolStripItemClickedEventArgs(value));
				}
			}
		}
	}

	public new Image Image
	{
		get
		{
			return OriginalImage;
		}
		set
		{
			Image image = base.Image;
			OriginalImage = value;
			if (value != null)
			{
				Bitmap bitmap = new Bitmap(value, ImageSize);
				if (base.Image != null)
				{
					base.Image.Dispose();
				}
				base.Image = bitmap;
			}
			else
			{
				base.Image = null;
			}
			image?.Dispose();
		}
	}

	private Size ImageSize
	{
		get
		{
			Size result = new Size(OriginalImage.Width, OriginalImage.Height);
			int num = base.Height - 8;
			if (result.Height > num)
			{
				result.Width = (int)((float)num / (float)result.Height * (float)result.Width);
				result.Height = num;
			}
			return result;
		}
	}

	protected override Padding DefaultPadding => new Padding(base.DefaultPadding.Left, base.DefaultPadding.Top, ArrowBoxWidth, base.DefaultPadding.Bottom);

	protected override Size DefaultSize => new Size(163, base.DefaultSize.Height);

	public event ToolStripItemClickedEventHandler ItemSelected;

	public MenuBox3()
	{
		base.TextImageRelation = TextImageRelation.ImageBeforeText;
		base.ImageAlign = ContentAlignment.MiddleRight;
		TextAlign = ContentAlignment.MiddleCenter;
		MenuClicked = DropDown_ItemClicked;
		MenuItemAdded = DropDown_ItemAdded;
		MenuItemRemoved = DropDown_ItemRemoved;
		AddMenu(DropDown);
		DropDown.Width = base.Width - ArrowBoxWidth;
		base.Resize += MenuBox3_Resize;
	}

	private void DropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		if (e.ClickedItem is ToolStripMenuItem)
		{
			SelectedItem = (ToolStripMenuItem)e.ClickedItem;
		}
	}

	private void DropDown_ItemAdded(object sender, ToolStripItemEventArgs e)
	{
		if (e.Item is ToolStripMenuItem)
		{
			AddMenu(((ToolStripMenuItem)e.Item).DropDown);
		}
	}

	private void DropDown_ItemRemoved(object sender, ToolStripItemEventArgs e)
	{
		if (e.Item is ToolStripMenuItem)
		{
			RemoveMenu(((ToolStripMenuItem)e.Item).DropDown);
		}
	}

	private void AddMenu(ToolStripDropDown menu)
	{
		if (menu == null)
		{
			return;
		}
		menu.ItemClicked += MenuClicked;
		menu.ItemAdded += MenuItemAdded;
		menu.ItemRemoved += MenuItemRemoved;
		for (int i = 0; i < menu.Items.Count; i++)
		{
			if (menu.Items[i] is ToolStripMenuItem)
			{
				AddMenu(((ToolStripMenuItem)menu.Items[i]).DropDown);
			}
		}
	}

	private void RemoveMenu(ToolStripDropDown menu)
	{
		if (menu == null)
		{
			return;
		}
		menu.ItemClicked -= MenuClicked;
		menu.ItemAdded -= MenuItemAdded;
		menu.ItemRemoved -= MenuItemRemoved;
		for (int i = 0; i < menu.Items.Count; i++)
		{
			if (menu.Items[i] is ToolStripMenuItem)
			{
				RemoveMenu(((ToolStripMenuItem)menu.Items[i]).DropDown);
			}
		}
	}

	private void MenuBox3_Resize(object sender, EventArgs e)
	{
		Image = OriginalImage;
	}

	protected override void OnPaint(PaintEventArgs pevent)
	{
		base.OnPaint(pevent);
		Graphics graphics = pevent.Graphics;
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.DrawLine(ArrowBoxPen, base.Width - ArrowBoxWidth, 1, base.Width - ArrowBoxWidth, base.Height - 2);
		graphics.FillPath(ArrowBrush, GetArrows(new Rectangle(base.Width - 18, 5, 10, base.Height - 12), 3));
	}

	private GraphicsPath GetArrows(Rectangle rect, int space)
	{
		GraphicsPath graphicsPath = new GraphicsPath(FillMode.Winding);
		float num = rect.Top + (rect.Height - space) / 2;
		PointF pointF = new PointF(rect.Left, num);
		PointF pointF2 = new PointF(rect.Left + rect.Width / 2, rect.Top);
		PointF pointF3 = new PointF(rect.Right, num);
		graphicsPath.AddPolygon(new PointF[3] { pointF, pointF2, pointF3 });
		num = (pointF.Y = num + (float)space);
		pointF2.Y = rect.Bottom;
		pointF3.Y = num;
		graphicsPath.AddPolygon(new PointF[3] { pointF, pointF2, pointF3 });
		return graphicsPath;
	}

	protected override void OnClick(EventArgs e)
	{
		base.OnClick(e);
		for (int i = 0; i < DropDown.Items.Count; i++)
		{
			DropDown.Items[i].Width = base.Width - ArrowBoxWidth - 1;
		}
		Point itemLocation = GetItemLocation(DropDown, CurrentItem);
		DropDown.Show(this, itemLocation);
	}

	private Point GetItemLocation(ToolStripDropDown menu, ToolStripMenuItem item)
	{
		Point point = new Point(1, 1);
		bool flag = false;
		for (int i = 0; i < menu.Items.Count; i++)
		{
			if (flag)
			{
				break;
			}
			ToolStripItem toolStripItem = menu.Items[i];
			flag = ((toolStripItem == item || !(toolStripItem is ToolStripMenuItem)) ? (toolStripItem == item) : (GetItemLocation(((ToolStripMenuItem)toolStripItem).DropDown, item) != point));
			if (flag)
			{
				float num = menu.ClientSize.Height / menu.Items.Count;
				point.Y = -(int)((float)i * num);
			}
		}
		return point;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (base.Image != null)
			{
				base.Image.Dispose();
			}
			DropDown.Dispose();
			ArrowBoxPen.Dispose();
			ArrowBrush.Dispose();
		}
		base.Dispose(disposing);
	}
}
