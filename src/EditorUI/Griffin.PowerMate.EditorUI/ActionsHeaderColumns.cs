using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.EditorUI;

internal class ActionsHeaderColumns : UserControl
{
	private IContainer components;

	private HeaderColumn ActionColumn;

	private HeaderColumn TypeColumn;

	private HeaderColumn DescriptionColumn;

	private int RowNumber = 6;

	private Color HideSelectColor = SystemColors.MenuBar;

	private AppNode myApplication;

	private CActionColumnItem.NodeChangeHandler AddNode;

	private CActionColumnItem.NodeChangeHandler RemoveNode;

	public Font HeaderFont
	{
		get
		{
			return ActionColumn.HeaderFont;
		}
		set
		{
			ActionColumn.HeaderFont = value;
			TypeColumn.HeaderFont = value;
			DescriptionColumn.HeaderFont = value;
		}
	}

	public Color HeaderColor
	{
		get
		{
			return ActionColumn.HeaderColor;
		}
		set
		{
			ActionColumn.HeaderColor = value;
			TypeColumn.HeaderColor = value;
			DescriptionColumn.HeaderColor = value;
		}
	}

	public Color BorderColor
	{
		get
		{
			return ActionColumn.BorderColor;
		}
		set
		{
			ActionColumn.BorderColor = value;
			TypeColumn.BorderColor = value;
			DescriptionColumn.BorderColor = value;
		}
	}

	public int RowHeight
	{
		get
		{
			return ActionColumn.RowHeight;
		}
		set
		{
			ActionColumn.RowHeight = value;
			TypeColumn.RowHeight = value;
			DescriptionColumn.RowHeight = value;
		}
	}

	public Color SelectRowColor
	{
		get
		{
			return ActionColumn.SelectRowColor;
		}
		set
		{
			ActionColumn.SelectRowColor = value;
			TypeColumn.SelectRowColor = value;
			DescriptionColumn.SelectRowColor = value;
			if (Focused)
			{
				Actions_Enter(this, null);
			}
		}
	}

	public Color HideSelectRowColor
	{
		get
		{
			return HideSelectColor;
		}
		set
		{
			HideSelectColor = value;
			if (!Focused)
			{
				Actions_Leave(this, null);
			}
		}
	}

	public ActionNode SelectedAction
	{
		get
		{
			if (myApplication != null && DescriptionColumn.SelectedItems.Count == 1)
			{
				return ((CActionColumnItem)DescriptionColumn.SelectedItems[0]).Node;
			}
			return null;
		}
	}

	public AppNode Application
	{
		set
		{
			myApplication = value;
			LoadActions();
		}
	}

	public event EventHandler ItemSelectionChanged;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		DisposeOfItems();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		new System.ComponentModel.ComponentResourceManager(typeof(Griffin.PowerMate.EditorUI.ActionsHeaderColumns));
		this.DescriptionColumn = new Griffin.PowerMate.EditorUI.HeaderColumn();
		this.TypeColumn = new Griffin.PowerMate.EditorUI.HeaderColumn();
		this.ActionColumn = new Griffin.PowerMate.EditorUI.HeaderColumn();
		base.SuspendLayout();
		this.DescriptionColumn.AltBackColor = System.Drawing.Color.FromArgb(237, 243, 254);
		this.DescriptionColumn.AlternateRowColor = true;
		this.DescriptionColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DescriptionColumn.BackColor = System.Drawing.SystemColors.Window;
		this.DescriptionColumn.BorderColor = System.Drawing.SystemColors.ActiveBorder;
		this.DescriptionColumn.BorderThickness = 1;
		this.DescriptionColumn.DrawBorder = true;
		this.DescriptionColumn.DrawRowLines = false;
		this.DescriptionColumn.HeaderAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.DescriptionColumn.HeaderColor = System.Drawing.Color.FromArgb(88, 87, 104);
		this.DescriptionColumn.HeaderFont = new System.Drawing.Font("Arial", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DescriptionColumn.HeaderText = "Description";
		this.DescriptionColumn.HideSelection = false;
		this.DescriptionColumn.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.DescriptionColumn.IconTextSpace = 3;
		this.DescriptionColumn.Indent = 3;
		this.DescriptionColumn.ItemAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.DescriptionColumn.Location = new System.Drawing.Point(150, 0);
		this.DescriptionColumn.MultiSelect = false;
		this.DescriptionColumn.Name = "DescriptionColumn";
		this.DescriptionColumn.RowHeight = 32;
		this.DescriptionColumn.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.DescriptionColumn.RowSpacing = 3;
		this.DescriptionColumn.SelectForeColor = System.Drawing.Color.White;
		this.DescriptionColumn.SelectionOutline = false;
		this.DescriptionColumn.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.DescriptionColumn.Size = new System.Drawing.Size(329, 255);
		this.DescriptionColumn.TabIndex = 2;
		this.DescriptionColumn.TextEdit = true;
		this.DescriptionColumn.Type = Griffin.PowerMate.EditorUI.ColumnType.TextOnly;
		this.DescriptionColumn.ItemSelectionChanged += new System.EventHandler(Column_ItemSelectionChanged);
		this.TypeColumn.AltBackColor = System.Drawing.Color.FromArgb(237, 243, 254);
		this.TypeColumn.AlternateRowColor = true;
		this.TypeColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.TypeColumn.BackColor = System.Drawing.SystemColors.Window;
		this.TypeColumn.BorderColor = System.Drawing.SystemColors.ActiveBorder;
		this.TypeColumn.BorderThickness = 1;
		this.TypeColumn.DrawBorder = true;
		this.TypeColumn.DrawRowLines = false;
		this.TypeColumn.HeaderAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.TypeColumn.HeaderColor = System.Drawing.Color.FromArgb(88, 87, 104);
		this.TypeColumn.HeaderFont = new System.Drawing.Font("Arial", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TypeColumn.HeaderText = "Type";
		this.TypeColumn.HideSelection = false;
		this.TypeColumn.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.TypeColumn.IconTextSpace = 3;
		this.TypeColumn.Indent = 3;
		this.TypeColumn.ItemAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.TypeColumn.Location = new System.Drawing.Point(75, 0);
		this.TypeColumn.MultiSelect = false;
		this.TypeColumn.Name = "TypeColumn";
		this.TypeColumn.RowHeight = 32;
		this.TypeColumn.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.TypeColumn.RowSpacing = 3;
		this.TypeColumn.SelectForeColor = System.Drawing.Color.Transparent;
		this.TypeColumn.SelectionOutline = false;
		this.TypeColumn.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.TypeColumn.Size = new System.Drawing.Size(76, 255);
		this.TypeColumn.TabIndex = 1;
		this.TypeColumn.TextEdit = false;
		this.TypeColumn.Type = Griffin.PowerMate.EditorUI.ColumnType.IconOnly;
		this.ActionColumn.AltBackColor = System.Drawing.Color.FromArgb(237, 243, 254);
		this.ActionColumn.AlternateRowColor = true;
		this.ActionColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ActionColumn.BackColor = System.Drawing.SystemColors.Window;
		this.ActionColumn.BorderColor = System.Drawing.SystemColors.ActiveBorder;
		this.ActionColumn.BorderThickness = 1;
		this.ActionColumn.DrawBorder = true;
		this.ActionColumn.DrawRowLines = false;
		this.ActionColumn.HeaderAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.ActionColumn.HeaderColor = System.Drawing.Color.FromArgb(88, 87, 104);
		this.ActionColumn.HeaderFont = new System.Drawing.Font("Arial", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ActionColumn.HeaderText = "Action";
		this.ActionColumn.HideSelection = false;
		this.ActionColumn.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.ActionColumn.IconTextSpace = 3;
		this.ActionColumn.Indent = 3;
		this.ActionColumn.ItemAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.ActionColumn.Location = new System.Drawing.Point(0, 0);
		this.ActionColumn.MultiSelect = false;
		this.ActionColumn.Name = "ActionColumn";
		this.ActionColumn.RowHeight = 32;
		this.ActionColumn.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.ActionColumn.RowSpacing = 3;
		this.ActionColumn.SelectForeColor = System.Drawing.Color.Transparent;
		this.ActionColumn.SelectionOutline = false;
		this.ActionColumn.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.ActionColumn.Size = new System.Drawing.Size(76, 255);
		this.ActionColumn.TabIndex = 0;
		this.ActionColumn.TextEdit = false;
		this.ActionColumn.Type = Griffin.PowerMate.EditorUI.ColumnType.IconOnly;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.Transparent;
		base.Controls.Add(this.DescriptionColumn);
		base.Controls.Add(this.TypeColumn);
		base.Controls.Add(this.ActionColumn);
		base.Name = "ActionsHeaderColumns";
		base.Size = new System.Drawing.Size(479, 255);
		base.Enter += new System.EventHandler(Actions_Enter);
		base.Leave += new System.EventHandler(Actions_Leave);
		base.ResumeLayout(false);
	}

	public ActionsHeaderColumns()
	{
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		InitializeComponent();
		AddNode = AddNodeToApp;
		RemoveNode = RemoveNodeToApp;
		InitializeRows();
		HideSelectColor = ActionColumn.HideSelectRowColor;
		DescriptionColumn.UserChangeText += UserChangeDescription;
	}

	public void InitializeRows()
	{
		for (int i = 0; i < RowNumber; i++)
		{
			CActionColumnItem cActionColumnItem = new CActionColumnItem(new ActionNode(GetRowPMAction(i), null));
			cActionColumnItem.AddNodeToCollection += AddNode;
			cActionColumnItem.RemoveNodeFromCollection += RemoveNode;
			ActionColumn.Items.Add(new PMActionColumnItem(cActionColumnItem));
			TypeColumn.Items.Add(cActionColumnItem);
			DescriptionColumn.Items.Add(cActionColumnItem);
		}
	}

	private void LoadActions()
	{
		for (int i = 0; i < RowNumber; i++)
		{
			LoadAction(i, ModifierKey.None);
		}
	}

	private void LoadAction(int row, ModifierKey modifier)
	{
		ActionNode actionNode = null;
		if (myApplication != null)
		{
			actionNode = myApplication.Find(GetRowPMAction(row), modifier);
		}
		if (actionNode == null)
		{
			actionNode = new ActionNode(GetRowPMAction(row), null);
		}
		TypeColumn.Items[row].Selected = false;
		((CActionColumnItem)TypeColumn.Items[row]).Node = actionNode;
	}

	private static PMAction GetRowPMAction(int row)
	{
		PMAction result = PMAction.Click;
		switch (row)
		{
		case 0:
			result = PMAction.ClockwiseRotate;
			break;
		case 1:
			result = PMAction.CounterClockwiseRotate;
			break;
		case 2:
			result = PMAction.ClickClockwiseRotate;
			break;
		case 3:
			result = PMAction.ClickCounterClockwiseRotate;
			break;
		case 4:
			result = PMAction.Click;
			break;
		case 5:
			result = PMAction.TimedClick;
			break;
		case 6:
			result = PMAction.DoubleClickClockwiseRotate;
			break;
		case 7:
			result = PMAction.DoubleClickCounterClockwiseRotate;
			break;
		case 8:
			result = PMAction.DoubleClick;
			break;
		case 9:
			result = PMAction.TimedDoubleClick;
			break;
		}
		return result;
	}

	private void Actions_Enter(object sender, EventArgs e)
	{
		ActionColumn.HideSelectRowColor = ActionColumn.SelectRowColor;
		TypeColumn.HideSelectRowColor = TypeColumn.SelectRowColor;
		DescriptionColumn.HideSelectRowColor = DescriptionColumn.SelectRowColor;
		foreach (CActionColumnItem item in DescriptionColumn.Items)
		{
			item.SelectedTextColor = Color.White;
		}
	}

	private void Actions_Leave(object sender, EventArgs e)
	{
		ActionColumn.HideSelectRowColor = HideSelectColor;
		TypeColumn.HideSelectRowColor = HideSelectColor;
		DescriptionColumn.HideSelectRowColor = HideSelectColor;
		foreach (CActionColumnItem item in DescriptionColumn.Items)
		{
			item.SelectedTextColor = Color.Transparent;
		}
	}

	private void Column_ItemSelectionChanged(object sender, EventArgs e)
	{
		if (this.ItemSelectionChanged != null)
		{
			this.ItemSelectionChanged(this, e);
		}
	}

	private void UserChangeDescription(object sender, IColumnItem item)
	{
		((CActionColumnItem)item).Node.Description = item.Text;
	}

	private void AddNodeToApp(CActionColumnItem sender, ActionNode node)
	{
		myApplication.Add(node);
	}

	private void RemoveNodeToApp(CActionColumnItem sender, ActionNode node)
	{
		myApplication.Remove(node);
	}

	private void DisposeOfItems()
	{
		foreach (PMActionColumnItem item in ActionColumn.Items)
		{
			item.Dispose();
		}
		foreach (CActionColumnItem item2 in TypeColumn.Items)
		{
			item2.Node = null;
		}
	}
}
