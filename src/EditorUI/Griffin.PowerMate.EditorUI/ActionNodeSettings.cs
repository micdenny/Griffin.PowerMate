using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class ActionNodeSettings : UserControl
{
	public delegate void ActionSelectedHandler(IComputerAction caction);

	private IContainer components;

	private MenuBox3 TypeSelect;

	private Label TypeLabel;

	private TrackBar SensitivitySlider;

	private Label SensitivityLabel;

	private Panel CActionPanel;

	private IPMActionPlugin[] AvailablePlugins = new IPMActionPlugin[0];

	private ActionNode myAction;

	private Panel CurrentPanel;

	private EventHandler UpdatePanel;

	private Brush NonActiveMessageBrush = SystemBrushes.Control;

	private Brush NonActiveTextBrush = SystemBrushes.WindowText;

	private string NonActiveMessage = "Select an action above to configure the setting.";

	private int NonActiveMessagePadding = 5;

	public IPMActionPlugin[] Plugins
	{
		set
		{
			AvailablePlugins = value;
			foreach (ToolStripMenuItem item in TypeSelect.Items)
			{
				item.Dispose();
			}
			TypeSelect.Items.Add(new CActionMenuItem(null));
			foreach (IPMActionPlugin iPMActionPlugin in value)
			{
				if (iPMActionPlugin.AvailableActions.Length != 1)
				{
					TypeSelect.Items.Add(new PluginMenuItem(iPMActionPlugin));
				}
				else
				{
					TypeSelect.Items.Add(new CActionMenuItem(iPMActionPlugin.AvailableActions[0]));
				}
			}
			if (myAction != null)
			{
				UpdateTypeSelect(myAction.Action, myAction.ComputerAction);
			}
		}
	}

	public ActionNode Action
	{
		set
		{
			myAction = value;
			Invalidate();
			if (value != null)
			{
				UpdateTypeSelect(value.Action, value.ComputerAction);
				if (value.Panel is IPMActionPanel)
				{
					((IPMActionPanel)value.Panel).Settings = value.PanelSettings;
				}
				SettingsPanel = value.Panel;
				Active = true;
			}
			else
			{
				SettingsPanel = null;
				Active = false;
			}
			SetSensitivitySlider(value);
		}
	}

	public uint Sensitivity
	{
		get
		{
			return (uint)(SensitivitySlider.Maximum - SensitivitySlider.Value);
		}
		set
		{
			int num = (int)(SensitivitySlider.Maximum - value);
			if (num > SensitivitySlider.Maximum)
			{
				num = SensitivitySlider.Maximum;
			}
			else if (num < SensitivitySlider.Minimum)
			{
				num = SensitivitySlider.Minimum;
			}
			SensitivitySlider.Value = num;
		}
	}

	private Panel SettingsPanel
	{
		set
		{
			if (CurrentPanel != null)
			{
				if (CurrentPanel is IPMActionPanel)
				{
					((IPMActionPanel)CurrentPanel).UpdateSettings -= UpdatePanel;
				}
				CActionPanel.Controls.Remove(CurrentPanel);
			}
			CurrentPanel = value;
			if (CurrentPanel != null)
			{
				if (CurrentPanel is IPMActionPanel)
				{
					((IPMActionPanel)CurrentPanel).UpdateSettings += UpdatePanel;
				}
				CActionPanel.Controls.Add(CurrentPanel);
			}
		}
	}

	public bool Active
	{
		set
		{
			if (value == (myAction != null))
			{
				TypeLabel.Visible = value;
				TypeSelect.Visible = value;
				CActionPanel.Visible = value;
			}
		}
	}

	public event ActionSelectedHandler ActionSelected;

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
		this.TypeLabel = new System.Windows.Forms.Label();
		this.SensitivitySlider = new System.Windows.Forms.TrackBar();
		this.SensitivityLabel = new System.Windows.Forms.Label();
		this.CActionPanel = new System.Windows.Forms.Panel();
		this.TypeSelect = new Griffin.PowerMate.EditorUI.MenuBox3();
		((System.ComponentModel.ISupportInitialize)this.SensitivitySlider).BeginInit();
		base.SuspendLayout();
		this.TypeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.TypeLabel.AutoSize = true;
		this.TypeLabel.Location = new System.Drawing.Point(102, 16);
		this.TypeLabel.Name = "TypeLabel";
		this.TypeLabel.Size = new System.Drawing.Size(34, 13);
		this.TypeLabel.TabIndex = 5;
		this.TypeLabel.Text = "Type:";
		this.TypeLabel.Visible = false;
		this.SensitivitySlider.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.SensitivitySlider.AutoSize = false;
		this.SensitivitySlider.Location = new System.Drawing.Point(169, 37);
		this.SensitivitySlider.Maximum = 24;
		this.SensitivitySlider.Name = "SensitivitySlider";
		this.SensitivitySlider.Size = new System.Drawing.Size(104, 31);
		this.SensitivitySlider.TabIndex = 6;
		this.SensitivitySlider.TickFrequency = 2;
		this.SensitivitySlider.Visible = false;
		this.SensitivitySlider.Scroll += new System.EventHandler(SensitivitySlider_Scroll);
		this.SensitivityLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.SensitivityLabel.AutoSize = true;
		this.SensitivityLabel.Location = new System.Drawing.Point(113, 42);
		this.SensitivityLabel.Name = "SensitivityLabel";
		this.SensitivityLabel.Size = new System.Drawing.Size(57, 13);
		this.SensitivityLabel.TabIndex = 7;
		this.SensitivityLabel.Text = "Sensitivity:";
		this.SensitivityLabel.Visible = false;
		this.CActionPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CActionPanel.BackColor = System.Drawing.Color.Transparent;
		this.CActionPanel.Location = new System.Drawing.Point(0, 71);
		this.CActionPanel.Name = "CActionPanel";
		this.CActionPanel.Size = new System.Drawing.Size(423, 133);
		this.CActionPanel.TabIndex = 8;
		this.TypeSelect.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.TypeSelect.Image = null;
		this.TypeSelect.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.TypeSelect.Location = new System.Drawing.Point(137, 11);
		this.TypeSelect.Name = "TypeSelect";
		this.TypeSelect.SelectedItem = null;
		this.TypeSelect.Size = new System.Drawing.Size(163, 23);
		this.TypeSelect.TabIndex = 3;
		this.TypeSelect.Text = "No Action";
		this.TypeSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.TypeSelect.UseVisualStyleBackColor = true;
		this.TypeSelect.Visible = false;
		this.TypeSelect.ItemSelected += new System.Windows.Forms.ToolStripItemClickedEventHandler(OnActionSelected);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.TypeLabel);
		base.Controls.Add(this.TypeSelect);
		base.Controls.Add(this.SensitivitySlider);
		base.Controls.Add(this.SensitivityLabel);
		base.Controls.Add(this.CActionPanel);
		base.Name = "ActionNodeSettings";
		base.Size = new System.Drawing.Size(423, 204);
		base.Resize += new System.EventHandler(ActionNodeSettings_Resize);
		((System.ComponentModel.ISupportInitialize)this.SensitivitySlider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public ActionNodeSettings()
	{
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		InitializeComponent();
		UpdatePanel = CurrentPanel_UpdatePanel;
		TypeSelect.ItemSelected += OnActionSelected;
	}

	private void UpdateTypeSelect(PMAction pmaction, IComputerAction selectedCAction)
	{
		CActionMenuItem cActionMenuItem = null;
		foreach (ToolStripMenuItem item in TypeSelect.Items)
		{
			if (item is PluginMenuItem)
			{
				bool flag = false;
				foreach (CActionMenuItem dropDownItem in item.DropDownItems)
				{
					if (dropDownItem.ComputerAction != null)
					{
						bool num = flag;
						bool flag2 = (dropDownItem.Shown = dropDownItem.ComputerAction.SupportsPMAction(pmaction));
						flag = num || flag2;
					}
					if (dropDownItem.Shown && selectedCAction == dropDownItem.ComputerAction)
					{
						cActionMenuItem = dropDownItem;
					}
				}
				item.Visible = flag;
			}
			else if (item is CActionMenuItem)
			{
				CActionMenuItem cActionMenuItem3 = (CActionMenuItem)item;
				if (cActionMenuItem3.ComputerAction != null)
				{
					cActionMenuItem3.Shown = cActionMenuItem3.ComputerAction.SupportsPMAction(pmaction);
				}
				if (cActionMenuItem3.Shown && selectedCAction == cActionMenuItem3.ComputerAction)
				{
					cActionMenuItem = cActionMenuItem3;
				}
			}
		}
		if (cActionMenuItem == null)
		{
			TypeSelect.Text = selectedCAction.Name;
		}
		else
		{
			TypeSelect.SelectedItem = cActionMenuItem;
		}
	}

	private void SetSensitivitySlider(ActionNode action)
	{
		bool flag = false;
		if (action != null)
		{
			flag = action.Action.ToString().Contains("Rotate");
			Sensitivity = action.Sensitivity;
		}
		Label sensitivityLabel = SensitivityLabel;
		bool visible = (SensitivitySlider.Visible = flag);
		sensitivityLabel.Visible = visible;
	}

	private void CurrentPanel_UpdatePanel(object sender, EventArgs e)
	{
		if (sender is IPMActionPanel)
		{
			myAction.PanelSettings = ((IPMActionPanel)sender).Settings;
		}
	}

	protected virtual void OnActionSelected(object sender, ToolStripItemClickedEventArgs e)
	{
		IComputerAction computerAction = null;
		if (e.ClickedItem is CActionMenuItem)
		{
			computerAction = ((CActionMenuItem)e.ClickedItem).ComputerAction;
		}
		if (myAction.ComputerAction == computerAction)
		{
			return;
		}
		myAction.ComputerAction = computerAction;
		if (computerAction != null)
		{
			if (computerAction.Panel is IPMActionPanel)
			{
				((IPMActionPanel)computerAction.Panel).Settings = null;
			}
			SettingsPanel = computerAction.Panel;
		}
		else
		{
			SettingsPanel = null;
		}
		if (this.ActionSelected != null)
		{
			this.ActionSelected(computerAction);
		}
	}

	private void SensitivitySlider_Scroll(object sender, EventArgs e)
	{
		myAction.Sensitivity = Sensitivity;
	}

	private void ActionNodeSettings_Resize(object sender, EventArgs e)
	{
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		if (myAction == null)
		{
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			SizeF sizeF = graphics.MeasureString(NonActiveMessage, Font);
			RectangleF rect = new RectangleF(((float)base.ClientSize.Width - sizeF.Width) / 2f - (float)NonActiveMessagePadding, ((float)base.ClientSize.Height - sizeF.Height) / 2f - (float)NonActiveMessagePadding, sizeF.Width + (float)(NonActiveMessagePadding * 2), sizeF.Height + (float)(NonActiveMessagePadding * 2));
			graphics.FillRectangle(NonActiveMessageBrush, rect);
			graphics.FillEllipse(NonActiveMessageBrush, rect.X - rect.Height / 2f, rect.Y, rect.Height, rect.Height);
			graphics.FillEllipse(NonActiveMessageBrush, rect.Right - rect.Height / 2f, rect.Y, rect.Height, rect.Height);
			graphics.DrawString(NonActiveMessage, Font, NonActiveTextBrush, rect.X + (float)NonActiveMessagePadding, rect.Y + (float)NonActiveMessagePadding);
		}
	}
}
