using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class DefaultApplicationSettings : Form
{
	private IContainer components;

	private ApplicationsHeaderColumn ApplicationsColumn;

	private Button AddButton;

	private Label InstructionsLabel;

	private DeviceNode _DefaultAppNodes = new DeviceNode();

	public AppCollection DefaultAppNodes
	{
		set
		{
			_DefaultAppNodes.Clear();
			foreach (AppNode item in value)
			{
				_DefaultAppNodes.Add(item);
			}
			ApplicationsColumn.Device = _DefaultAppNodes;
		}
	}

	public ColumnItemCollection SelectedItems => ApplicationsColumn.SelectedItems;

	public event EventHandler<DefaultAppEventArgs> AddDefault;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Griffin.PowerMate.EditorUI.DefaultApplicationSettings));
		this.AddButton = new System.Windows.Forms.Button();
		this.InstructionsLabel = new System.Windows.Forms.Label();
		this.ApplicationsColumn = new Griffin.PowerMate.EditorUI.ApplicationsHeaderColumn();
		base.SuspendLayout();
		this.AddButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.AddButton.Location = new System.Drawing.Point(101, 251);
		this.AddButton.Name = "AddButton";
		this.AddButton.Size = new System.Drawing.Size(75, 23);
		this.AddButton.TabIndex = 1;
		this.AddButton.Text = "Add";
		this.AddButton.UseVisualStyleBackColor = true;
		this.AddButton.Click += new System.EventHandler(AddButton_Click);
		this.InstructionsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.InstructionsLabel.Location = new System.Drawing.Point(26, 9);
		this.InstructionsLabel.Name = "InstructionsLabel";
		this.InstructionsLabel.Size = new System.Drawing.Size(223, 32);
		this.InstructionsLabel.TabIndex = 3;
		this.InstructionsLabel.Text = "Select default settings below and click \"Add\" to add them to the selected PowerMates. ";
		this.InstructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ApplicationsColumn.AltBackColor = System.Drawing.Color.FromArgb(237, 243, 254);
		this.ApplicationsColumn.AlternateRowColor = true;
		this.ApplicationsColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ApplicationsColumn.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
		this.ApplicationsColumn.BackColor = System.Drawing.SystemColors.Window;
		this.ApplicationsColumn.BorderColor = System.Drawing.SystemColors.HotTrack;
		this.ApplicationsColumn.BorderThickness = 1;
		this.ApplicationsColumn.DrawBorder = true;
		this.ApplicationsColumn.DrawRowLines = false;
		this.ApplicationsColumn.HeaderAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.ApplicationsColumn.HeaderColor = System.Drawing.SystemColors.ControlText;
		this.ApplicationsColumn.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ApplicationsColumn.HeaderImage = (System.Drawing.Bitmap)resources.GetObject("ApplicationsColumn.HeaderImage");
		this.ApplicationsColumn.HeaderImageLayout = System.Windows.Forms.ImageLayout.Tile;
		this.ApplicationsColumn.HeaderText = "Applications";
		this.ApplicationsColumn.HideSelection = false;
		this.ApplicationsColumn.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.ApplicationsColumn.IconTextSpace = 3;
		this.ApplicationsColumn.Indent = 3;
		this.ApplicationsColumn.ItemAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.ApplicationsColumn.Location = new System.Drawing.Point(15, 47);
		this.ApplicationsColumn.MultiSelect = true;
		this.ApplicationsColumn.Name = "ApplicationsColumn";
		this.ApplicationsColumn.RowHeight = 24;
		this.ApplicationsColumn.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.ApplicationsColumn.RowSpacing = 3;
		this.ApplicationsColumn.SelectForeColor = System.Drawing.Color.Transparent;
		this.ApplicationsColumn.SelectionOutline = true;
		this.ApplicationsColumn.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.ApplicationsColumn.Size = new System.Drawing.Size(244, 192);
		this.ApplicationsColumn.TabIndex = 0;
		this.ApplicationsColumn.TextEdit = false;
		this.ApplicationsColumn.Type = Griffin.PowerMate.EditorUI.ColumnType.IconAndText;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(273, 286);
		base.Controls.Add(this.ApplicationsColumn);
		base.Controls.Add(this.InstructionsLabel);
		base.Controls.Add(this.AddButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
		base.Icon = Griffin.PowerMate.EditorUI.Properties.Resources.PowerMate;
		this.MinimumSize = new System.Drawing.Size(281, 194);
		base.Name = "DefaultApplicationSettings";
		this.Text = "Default Application Settings";
		base.ResumeLayout(false);
	}

	public DefaultApplicationSettings()
	{
		InitializeComponent();
	}

	private void AddButton_Click(object sender, EventArgs e)
	{
		ColumnItemCollection selectedItems = SelectedItems;
		AppNode[] array = new AppNode[selectedItems.Count];
		for (int i = 0; i < selectedItems.Count; i++)
		{
			array[i] = ((ApplicationColumnItem)selectedItems[i]).Node;
		}
		OnAddDefault(new DefaultAppEventArgs(array));
	}

	protected virtual void OnAddDefault(DefaultAppEventArgs e)
	{
		if (this.AddDefault != null)
		{
			this.AddDefault(this, e);
		}
	}
}
