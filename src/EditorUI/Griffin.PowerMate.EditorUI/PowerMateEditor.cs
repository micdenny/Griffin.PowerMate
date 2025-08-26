using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class PowerMateEditor : Form
{
	private IContainer components;

	private ActionsHeaderColumns ActionList;

	private ApplicationsHeaderColumn ApplicationList;

	private PowerMatesHeaderColumn PowerMateList;

	private MenuStrip MainMenu;

	private ContextMenuStrip PowerMateMenu;

	private ActionNodeSettings ActionSettings;

	private ContextMenuStrip ApplicationMenu;

	private ToolStripMenuItem ConfigurePowerMateMenuItem;

	private ToolStripSeparator PowerMateMenuSeparator1;

	private ToolStripMenuItem AddPowerMateMenuItem;

	private ToolStripMenuItem RemovePowerMateMenuItem;

	private ToolStripMenuItem RenamePowerMateMenuItem;

	private ToolStripMenuItem AddApplicationMenuItem;

	private ToolStripMenuItem RemoveApplicationMenuItem;

	private ToolStripMenuItem RenameApplicationMenuItem;

	private StatusStrip PMStatusStrip;

	private ToolStripSeparator PowerMateMenuSeparator2;

	private ToolStripMenuItem SwitchPowerMateMenuItem;

	private ToolStripMenuItem ReplacePowerMateMenuItem;

	private ToolStripMenuItem DuplicatePowerMateMenuItem;

	private ToolStripSeparator PowerMateMenuSeparator3;

	private ToolStripMenuItem ImportPowerMateMenuItem;

	private ToolStripMenuItem ExportPowerMateMenuItem;

	private ToolStripMenuItem DefaultApplicationMenuItem;

	private Timer HidePowerMatesTimer;

	private ToolStripSeparator ApplicationMenuSeparator1;

	private ToolStripSeparator ApplicationMenuSeparator2;

	private ToolStripMenuItem CopyApplicationMenuItem;

	private ToolStripMenuItem PasteApplicationMenuItem;

	private ToolTip EditorToolTip;

	private ToolStripMenuItem FileMenuItem;

	private ToolStripMenuItem ExportFileMenuItem;

	private ToolStripMenuItem RevertFileMenuItem;

	private ToolStripSeparator toolStripMenuItem1;

	private ToolStripMenuItem CloseFileMenuItem;

	private ToolStripMenuItem HelpMenuItem;

	private ToolStripMenuItem PowerMatesMenuItem;

	private ToolStripMenuItem ApplicationsMenuItem;

	private SaveFileDialog ExportSettingsDialog;

	private ToolStripMenuItem ChangeIconApplicationMenuItem;

	private ToolStripMenuItem AboutPowerMateMenuItem;

	private ToolStripMenuItem ViewManualMenuItem;

	private string ManualFilename = "PowerMate 2 Windows Manual.pdf";

	private bool PowerMatesHidden;

	private int PMPixelChange = 20;

	private ConfigurePowerMate PMConfigure = new ConfigurePowerMate();

	private AddApplicationDialog AddAppDialog = new AddApplicationDialog();

	private DefaultApplicationSettings DefaultApps = new DefaultApplicationSettings();

	private AboutPowerMate AboutPowerMateForm;

	private Image AddAppImage = Resources.add;

	private Image HidePowerMatesImage = Resources.hidePowerMates;

	private Image ShowPowermatesImage = Resources.showPowerMates;

	private ToolStripButton AddApplicationButton;

	private ToolStripButton HidePowerMatesButton;

	private ToolStripStatusLabel CopyrightLabel;

	private PowerMateDoc PMDoc;

	private AppNode[] CopiedAppNodes;

	private PowerMateDoc OriginalPMDoc;

	public Font HeaderFont
	{
		get
		{
			return ApplicationList.HeaderFont;
		}
		set
		{
			PowerMateList.HeaderFont = value;
			ApplicationList.HeaderFont = value;
			ActionList.HeaderFont = value;
		}
	}

	public Color HeaderColor
	{
		get
		{
			return ApplicationList.HeaderColor;
		}
		set
		{
			PowerMateList.HeaderColor = value;
			ApplicationList.HeaderColor = value;
			ActionList.HeaderColor = value;
		}
	}

	public int RowHeight
	{
		get
		{
			return ApplicationList.RowHeight;
		}
		set
		{
			PowerMateList.RowHeight = value;
			ApplicationList.RowHeight = value;
			ActionList.RowHeight = value;
		}
	}

	public PowerMateDoc PowerMateDoc
	{
		set
		{
			if (value != PMDoc)
			{
				PMDoc = value;
				PowerMateList.PmDoc = value;
			}
		}
	}

	public AppCollection DefaultAppNodes
	{
		set
		{
			DefaultApps.DefaultAppNodes = value;
		}
	}

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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Griffin.PowerMate.EditorUI.PowerMateEditor));
		this.MainMenu = new System.Windows.Forms.MenuStrip();
		this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.RevertFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
		this.CloseFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PowerMatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ApplicationsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ViewManualMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.AboutPowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ApplicationMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.DefaultApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ApplicationMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.AddApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.RemoveApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.RenameApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ChangeIconApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ApplicationMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.CopyApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PasteApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PowerMateMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.ConfigurePowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PowerMateMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.AddPowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.RemovePowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.RenamePowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PowerMateMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.SwitchPowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ReplacePowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.DuplicatePowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PowerMateMenuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.ImportPowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportPowerMateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PMStatusStrip = new System.Windows.Forms.StatusStrip();
		this.HidePowerMatesTimer = new System.Windows.Forms.Timer(this.components);
		this.EditorToolTip = new System.Windows.Forms.ToolTip(this.components);
		this.ExportSettingsDialog = new System.Windows.Forms.SaveFileDialog();
		this.ApplicationList = new Griffin.PowerMate.EditorUI.ApplicationsHeaderColumn();
		this.PowerMateList = new Griffin.PowerMate.EditorUI.PowerMatesHeaderColumn();
		this.ActionList = new Griffin.PowerMate.EditorUI.ActionsHeaderColumns();
		this.ActionSettings = new Griffin.PowerMate.EditorUI.ActionNodeSettings();
		this.MainMenu.SuspendLayout();
		this.ApplicationMenu.SuspendLayout();
		this.PowerMateMenu.SuspendLayout();
		base.SuspendLayout();
		this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.FileMenuItem, this.PowerMatesMenuItem, this.ApplicationsMenuItem, this.HelpMenuItem });
		this.MainMenu.Location = new System.Drawing.Point(0, 0);
		this.MainMenu.Name = "MainMenu";
		this.MainMenu.Size = new System.Drawing.Size(580, 24);
		this.MainMenu.TabIndex = 5;
		this.MainMenu.Text = "menuStrip1";
		this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.ExportFileMenuItem, this.RevertFileMenuItem, this.toolStripMenuItem1, this.CloseFileMenuItem });
		this.FileMenuItem.Name = "FileMenuItem";
		this.FileMenuItem.Size = new System.Drawing.Size(35, 20);
		this.FileMenuItem.Text = "File";
		this.ExportFileMenuItem.Name = "ExportFileMenuItem";
		this.ExportFileMenuItem.Size = new System.Drawing.Size(129, 22);
		this.ExportFileMenuItem.Text = "Export...";
		this.ExportFileMenuItem.Click += new System.EventHandler(ExportFileMenuItem_Click);
		this.RevertFileMenuItem.Name = "RevertFileMenuItem";
		this.RevertFileMenuItem.Size = new System.Drawing.Size(129, 22);
		this.RevertFileMenuItem.Text = "Revert";
		this.RevertFileMenuItem.Click += new System.EventHandler(RevertFileMenuItem_Click);
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 6);
		this.CloseFileMenuItem.Name = "CloseFileMenuItem";
		this.CloseFileMenuItem.Size = new System.Drawing.Size(129, 22);
		this.CloseFileMenuItem.Text = "Close";
		this.CloseFileMenuItem.Click += new System.EventHandler(CloseFileMenuItem_Click);
		this.PowerMatesMenuItem.Name = "PowerMatesMenuItem";
		this.PowerMatesMenuItem.Size = new System.Drawing.Size(78, 20);
		this.PowerMatesMenuItem.Text = "PowerMates";
		this.ApplicationsMenuItem.Name = "ApplicationsMenuItem";
		this.ApplicationsMenuItem.Size = new System.Drawing.Size(76, 20);
		this.ApplicationsMenuItem.Text = "Applications";
		this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.ViewManualMenuItem, this.AboutPowerMateMenuItem });
		this.HelpMenuItem.Name = "HelpMenuItem";
		this.HelpMenuItem.Size = new System.Drawing.Size(40, 20);
		this.HelpMenuItem.Text = "Help";
		this.ViewManualMenuItem.Name = "ViewManualMenuItem";
		this.ViewManualMenuItem.Size = new System.Drawing.Size(171, 22);
		this.ViewManualMenuItem.Text = "View Manual...";
		this.ViewManualMenuItem.Click += new System.EventHandler(ViewManualMenuItem_Click);
		this.AboutPowerMateMenuItem.Name = "AboutPowerMateMenuItem";
		this.AboutPowerMateMenuItem.Size = new System.Drawing.Size(171, 22);
		this.AboutPowerMateMenuItem.Text = "About PowerMate";
		this.AboutPowerMateMenuItem.Click += new System.EventHandler(AboutPowerMateMenuItem_Click);
		this.ApplicationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.DefaultApplicationMenuItem, this.ApplicationMenuSeparator1, this.AddApplicationMenuItem, this.RemoveApplicationMenuItem, this.RenameApplicationMenuItem, this.ChangeIconApplicationMenuItem, this.ApplicationMenuSeparator2, this.CopyApplicationMenuItem, this.PasteApplicationMenuItem });
		this.ApplicationMenu.Name = "ApplicationMenu";
		this.ApplicationMenu.ShowImageMargin = false;
		this.ApplicationMenu.Size = new System.Drawing.Size(150, 170);
		this.ApplicationMenu.Opening += new System.ComponentModel.CancelEventHandler(ApplicationMenu_Opening);
		this.DefaultApplicationMenuItem.Name = "DefaultApplicationMenuItem";
		this.DefaultApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.DefaultApplicationMenuItem.Text = "Default Settings...";
		this.DefaultApplicationMenuItem.Click += new System.EventHandler(DefaultApplicationMenuItem_Click);
		this.ApplicationMenuSeparator1.Name = "ApplicationMenuSeparator1";
		this.ApplicationMenuSeparator1.Size = new System.Drawing.Size(146, 6);
		this.AddApplicationMenuItem.Enabled = false;
		this.AddApplicationMenuItem.Name = "AddApplicationMenuItem";
		this.AddApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.AddApplicationMenuItem.Text = "Add";
		this.AddApplicationMenuItem.Click += new System.EventHandler(AddApplication_Click);
		this.RemoveApplicationMenuItem.Name = "RemoveApplicationMenuItem";
		this.RemoveApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.RemoveApplicationMenuItem.Text = "Remove";
		this.RemoveApplicationMenuItem.Click += new System.EventHandler(RemoveApplications_Click);
		this.RenameApplicationMenuItem.Name = "RenameApplicationMenuItem";
		this.RenameApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.RenameApplicationMenuItem.Text = "Rename";
		this.RenameApplicationMenuItem.Click += new System.EventHandler(RenameApplication_Click);
		this.ChangeIconApplicationMenuItem.Name = "ChangeIconApplicationMenuItem";
		this.ChangeIconApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.ChangeIconApplicationMenuItem.Text = "Change Icon...";
		this.ChangeIconApplicationMenuItem.Click += new System.EventHandler(ChangeIconApplicationMenuItem_Click);
		this.ApplicationMenuSeparator2.Name = "ApplicationMenuSeparator2";
		this.ApplicationMenuSeparator2.Size = new System.Drawing.Size(146, 6);
		this.CopyApplicationMenuItem.Name = "CopyApplicationMenuItem";
		this.CopyApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.CopyApplicationMenuItem.Text = "Copy Settings";
		this.CopyApplicationMenuItem.Click += new System.EventHandler(CopyApplicationMenuItem_Click);
		this.PasteApplicationMenuItem.Enabled = false;
		this.PasteApplicationMenuItem.Name = "PasteApplicationMenuItem";
		this.PasteApplicationMenuItem.Size = new System.Drawing.Size(149, 22);
		this.PasteApplicationMenuItem.Text = "Paste Settings";
		this.PasteApplicationMenuItem.Click += new System.EventHandler(PasteApplicationMenuItem_Click);
		this.PowerMateMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.ConfigurePowerMateMenuItem, this.PowerMateMenuSeparator1, this.AddPowerMateMenuItem, this.RemovePowerMateMenuItem, this.RenamePowerMateMenuItem, this.PowerMateMenuSeparator2, this.SwitchPowerMateMenuItem, this.ReplacePowerMateMenuItem, this.DuplicatePowerMateMenuItem, this.PowerMateMenuSeparator3,
			this.ImportPowerMateMenuItem, this.ExportPowerMateMenuItem
		});
		this.PowerMateMenu.Name = "contextMenuStrip1";
		this.PowerMateMenu.ShowImageMargin = false;
		this.PowerMateMenu.Size = new System.Drawing.Size(159, 220);
		this.PowerMateMenu.Opening += new System.ComponentModel.CancelEventHandler(PowerMateMenu_Opening);
		this.ConfigurePowerMateMenuItem.Name = "ConfigurePowerMateMenuItem";
		this.ConfigurePowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.ConfigurePowerMateMenuItem.Text = "Configure...";
		this.ConfigurePowerMateMenuItem.Click += new System.EventHandler(ConfigureMenuItem_Click);
		this.PowerMateMenuSeparator1.Name = "PowerMateMenuSeparator1";
		this.PowerMateMenuSeparator1.Size = new System.Drawing.Size(155, 6);
		this.AddPowerMateMenuItem.Name = "AddPowerMateMenuItem";
		this.AddPowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.AddPowerMateMenuItem.Text = "Add";
		this.AddPowerMateMenuItem.Click += new System.EventHandler(AddPowerMate_Click);
		this.RemovePowerMateMenuItem.Name = "RemovePowerMateMenuItem";
		this.RemovePowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.RemovePowerMateMenuItem.Text = "Remove";
		this.RemovePowerMateMenuItem.Click += new System.EventHandler(RemovePowerMates_Click);
		this.RenamePowerMateMenuItem.Name = "RenamePowerMateMenuItem";
		this.RenamePowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.RenamePowerMateMenuItem.Text = "Rename";
		this.RenamePowerMateMenuItem.Click += new System.EventHandler(RenamePowerMate_Click);
		this.PowerMateMenuSeparator2.Name = "PowerMateMenuSeparator2";
		this.PowerMateMenuSeparator2.Size = new System.Drawing.Size(155, 6);
		this.SwitchPowerMateMenuItem.Name = "SwitchPowerMateMenuItem";
		this.SwitchPowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.SwitchPowerMateMenuItem.Text = "Switch Setting with";
		this.SwitchPowerMateMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(SwitchPowerMateMenuItem_DropDownItemClicked);
		this.ReplacePowerMateMenuItem.Name = "ReplacePowerMateMenuItem";
		this.ReplacePowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.ReplacePowerMateMenuItem.Text = "Replace Setting with";
		this.ReplacePowerMateMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(ReplacePowerMateMenuItem_DropDownItemClicked);
		this.DuplicatePowerMateMenuItem.Name = "DuplicatePowerMateMenuItem";
		this.DuplicatePowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.DuplicatePowerMateMenuItem.Text = "Duplicate Setting";
		this.DuplicatePowerMateMenuItem.Click += new System.EventHandler(DuplicatePowerMateMenuItem_Click);
		this.PowerMateMenuSeparator3.Name = "PowerMateMenuSeparator3";
		this.PowerMateMenuSeparator3.Size = new System.Drawing.Size(155, 6);
		this.ImportPowerMateMenuItem.Name = "ImportPowerMateMenuItem";
		this.ImportPowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.ImportPowerMateMenuItem.Text = "Import Setting...";
		this.ImportPowerMateMenuItem.Click += new System.EventHandler(ImportPowerMateMenuItem_Click);
		this.ExportPowerMateMenuItem.Name = "ExportPowerMateMenuItem";
		this.ExportPowerMateMenuItem.Size = new System.Drawing.Size(158, 22);
		this.ExportPowerMateMenuItem.Text = "Export Setting...";
		this.ExportPowerMateMenuItem.Click += new System.EventHandler(ExportPowerMateMenuItem_Click);
		this.PMStatusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
		this.PMStatusStrip.Location = new System.Drawing.Point(0, 358);
		this.PMStatusStrip.Name = "PMStatusStrip";
		this.PMStatusStrip.ShowItemToolTips = true;
		this.PMStatusStrip.Size = new System.Drawing.Size(580, 22);
		this.PMStatusStrip.TabIndex = 7;
		this.HidePowerMatesTimer.Interval = 35;
		this.HidePowerMatesTimer.Tick += new System.EventHandler(HidePowerMatesTimer_Tick);
		this.ExportSettingsDialog.DefaultExt = "pmsettings";
		this.ExportSettingsDialog.FileName = "My Settings";
		this.ExportSettingsDialog.Filter = "PowerMate Settings|*.pmsettings|All Files|*.*";
		this.ExportSettingsDialog.SupportMultiDottedExtensions = true;
		this.ExportSettingsDialog.Title = "Export PowerMate Settings";
		this.ApplicationList.AllowDrop = true;
		this.ApplicationList.AltBackColor = System.Drawing.Color.AliceBlue;
		this.ApplicationList.AlternateRowColor = false;
		this.ApplicationList.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
		this.ApplicationList.BackColor = System.Drawing.SystemColors.Window;
		this.ApplicationList.BorderColor = System.Drawing.SystemColors.ActiveBorder;
		this.ApplicationList.BorderThickness = 1;
		this.ApplicationList.ContextMenuStrip = this.ApplicationMenu;
		this.ApplicationList.DrawBorder = true;
		this.ApplicationList.DrawRowLines = false;
		this.ApplicationList.HeaderAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.ApplicationList.HeaderColor = System.Drawing.Color.FromArgb(88, 87, 104);
		this.ApplicationList.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ApplicationList.HeaderImage = (System.Drawing.Bitmap)resources.GetObject("ApplicationList.HeaderImage");
		this.ApplicationList.HeaderImageLayout = System.Windows.Forms.ImageLayout.Tile;
		this.ApplicationList.HeaderText = "Applications";
		this.ApplicationList.HideSelection = false;
		this.ApplicationList.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.ApplicationList.IconTextSpace = 3;
		this.ApplicationList.Indent = 3;
		this.ApplicationList.ItemAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.ApplicationList.Location = new System.Drawing.Point(-1, 24);
		this.ApplicationList.MultiSelect = true;
		this.ApplicationList.Name = "ApplicationList";
		this.ApplicationList.RowHeight = 32;
		this.ApplicationList.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.ApplicationList.RowSpacing = 3;
		this.ApplicationList.SelectForeColor = System.Drawing.Color.White;
		this.ApplicationList.SelectionOutline = true;
		this.ApplicationList.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.ApplicationList.Size = new System.Drawing.Size(178, 215);
		this.ApplicationList.TabIndex = 3;
		this.ApplicationList.TextEdit = false;
		this.ApplicationList.Type = Griffin.PowerMate.EditorUI.ColumnType.IconAndIndentText;
		this.PowerMateList.AltBackColor = System.Drawing.Color.AliceBlue;
		this.PowerMateList.AlternateRowColor = false;
		this.PowerMateList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.PowerMateList.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
		this.PowerMateList.BackColor = System.Drawing.SystemColors.Window;
		this.PowerMateList.BorderColor = System.Drawing.SystemColors.ActiveBorder;
		this.PowerMateList.BorderThickness = 1;
		this.PowerMateList.ContextMenuStrip = this.PowerMateMenu;
		this.PowerMateList.DrawBorder = true;
		this.PowerMateList.DrawRowLines = false;
		this.PowerMateList.HeaderAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.PowerMateList.HeaderColor = System.Drawing.Color.FromArgb(88, 87, 104);
		this.PowerMateList.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PowerMateList.HeaderImage = (System.Drawing.Bitmap)resources.GetObject("PowerMateList.HeaderImage");
		this.PowerMateList.HeaderImageLayout = System.Windows.Forms.ImageLayout.Tile;
		this.PowerMateList.HeaderText = "PowerMates";
		this.PowerMateList.HideSelection = false;
		this.PowerMateList.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.PowerMateList.IconTextSpace = 3;
		this.PowerMateList.Indent = 3;
		this.PowerMateList.ItemAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.PowerMateList.Location = new System.Drawing.Point(-1, 238);
		this.PowerMateList.MultiSelect = false;
		this.PowerMateList.Name = "PowerMateList";
		this.PowerMateList.RowHeight = 32;
		this.PowerMateList.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.PowerMateList.RowSpacing = 3;
		this.PowerMateList.SelectForeColor = System.Drawing.Color.White;
		this.PowerMateList.SelectionOutline = true;
		this.PowerMateList.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.PowerMateList.Size = new System.Drawing.Size(178, 121);
		this.PowerMateList.TabIndex = 4;
		this.PowerMateList.TextEdit = false;
		this.PowerMateList.Type = Griffin.PowerMate.EditorUI.ColumnType.IconAndText;
		this.ActionList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ActionList.BackColor = System.Drawing.Color.Transparent;
		this.ActionList.BorderColor = System.Drawing.SystemColors.ActiveBorder;
		this.ActionList.HeaderColor = System.Drawing.Color.FromArgb(88, 87, 104);
		this.ActionList.HeaderFont = new System.Drawing.Font("Arial", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ActionList.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.ActionList.Location = new System.Drawing.Point(176, 24);
		this.ActionList.Name = "ActionList";
		this.ActionList.RowHeight = 32;
		this.ActionList.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.ActionList.Size = new System.Drawing.Size(404, 215);
		this.ActionList.TabIndex = 2;
		this.ActionSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ActionSettings.BackColor = System.Drawing.SystemColors.Window;
		this.ActionSettings.Location = new System.Drawing.Point(176, 239);
		this.ActionSettings.Name = "ActionSettings";
		this.ActionSettings.Sensitivity = 0u;
		this.ActionSettings.Size = new System.Drawing.Size(404, 120);
		this.ActionSettings.TabIndex = 6;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(580, 380);
		base.Controls.Add(this.PMStatusStrip);
		base.Controls.Add(this.ApplicationList);
		base.Controls.Add(this.MainMenu);
		base.Controls.Add(this.PowerMateList);
		base.Controls.Add(this.ActionList);
		base.Controls.Add(this.ActionSettings);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MainMenuStrip = this.MainMenu;
		base.MaximizeBox = false;
		base.Name = "PowerMateEditor";
		this.Text = "PowerMate";
		this.MainMenu.ResumeLayout(false);
		this.MainMenu.PerformLayout();
		this.ApplicationMenu.ResumeLayout(false);
		this.PowerMateMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public PowerMateEditor(PowerMateDoc powerMateDoc)
	{
		InitializeComponent();
		components.Add(PMConfigure);
		components.Add(AddAppDialog);
		components.Add(DefaultApps);
		InitializeStatusStrip();
		InitializePowerMateMenus();
		HeaderColor = (HeaderColor = Color.FromArgb(88, 87, 104));
		HeaderFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
		((ToolStripDropDownMenu)FileMenuItem.DropDown).ShowImageMargin = false;
		((ToolStripDropDownMenu)HelpMenuItem.DropDown).ShowImageMargin = false;
		PowerMatesMenuItem.DropDown = PowerMateMenu;
		ApplicationsMenuItem.DropDown = ApplicationMenu;
		PowerMateList.ItemSelectionChanged += PowerMateList_ItemSelectionChanged;
		ApplicationList.ItemSelectionChanged += ApplicationList_ItemSelectionChanged;
		ActionList.ItemSelectionChanged += ActionList_ItemSelectionChanged;
		ActionSettings.Plugins = PowerMateApp.ActionPlugins;
		PMConfigure.FormClosing += PMConfigure_FormClosing;
		DefaultApps.FormClosing += DefaultApps_FormClosing;
		DefaultApps.AddDefault += DefaultApps_AddDefault;
		OriginalPMDoc = (PowerMateDoc)powerMateDoc.Clone();
		PowerMateDoc = powerMateDoc;
		if (PowerMateList.Items.Count == 1)
		{
			PowerMateList.Top = PMStatusStrip.Top;
			HidePowerMates();
		}
	}

	private void InitializeStatusStrip()
	{
		AddApplicationButton = new ToolStripButton(null, AddAppImage, AddApplication_Click);
		ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
		HidePowerMatesButton = new ToolStripButton(null, HidePowerMatesImage, HidePowerMatesButton_Click);
		ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
		CopyrightLabel = new ToolStripStatusLabel("© Copyright 2007 Griffin Technology, Inc.");
		AddApplicationButton.ImageScaling = ToolStripItemImageScaling.None;
		AddApplicationButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
		AddApplicationButton.Text = "Add a setting for an application";
		AddApplicationButton.Enabled = false;
		HidePowerMatesButton.ImageScaling = ToolStripItemImageScaling.None;
		HidePowerMatesButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
		HidePowerMatesButton.Text = "Show / Hide PowerMates";
		CopyrightLabel.Spring = true;
		CopyrightLabel.TextAlign = ContentAlignment.MiddleRight;
		PMStatusStrip.Items.AddRange(new ToolStripItem[5] { AddApplicationButton, toolStripSeparator, HidePowerMatesButton, toolStripSeparator2, CopyrightLabel });
	}

	private void PMConfigure_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (e.CloseReason == CloseReason.UserClosing)
		{
			e.Cancel = true;
			PMConfigure.Hide();
		}
	}

	private void DefaultApps_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (e.CloseReason == CloseReason.UserClosing)
		{
			e.Cancel = true;
			DefaultApps.Hide();
		}
	}

	private void DefaultApps_AddDefault(object sender, DefaultAppEventArgs e)
	{
		foreach (DeviceColumnItem selectedItem in PowerMateList.SelectedItems)
		{
			AppNode[] appNodes = e.AppNodes;
			foreach (AppNode appNode in appNodes)
			{
				if (!selectedItem.Node.Contains(appNode.Image))
				{
					selectedItem.Node.Add(appNode);
				}
				else
				{
					ApplicationList.ShowSettingExistsMessage(selectedItem.Text, appNode.Image);
				}
			}
		}
	}

	private void PowerMateList_ItemSelectionChanged(object sender, EventArgs e)
	{
		bool flag = PowerMateList.SelectedItems.Count == 1;
		if (flag)
		{
			ApplicationList.Device = ((DeviceColumnItem)PowerMateList.SelectedItems[0]).Node;
		}
		else
		{
			ApplicationList.Device = null;
		}
		ToolStripButton addApplicationButton = AddApplicationButton;
		bool enabled = (AddApplicationMenuItem.Enabled = flag);
		addApplicationButton.Enabled = enabled;
		PasteApplicationMenuItem.Enabled = CopiedAppNodes != null && flag;
	}

	private void ApplicationList_ItemSelectionChanged(object sender, EventArgs e)
	{
		if (ApplicationList.SelectedItems.Count == 1)
		{
			ActionList.Application = ((ApplicationColumnItem)ApplicationList.SelectedItems[0]).Node;
		}
		else
		{
			ActionList.Application = null;
		}
	}

	private void ActionList_ItemSelectionChanged(object sender, EventArgs e)
	{
		ActionSettings.Action = ActionList.SelectedAction;
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		PowerMateList.PmDoc = null;
		AddAppImage.Dispose();
		HidePowerMatesImage.Dispose();
		ShowPowermatesImage.Dispose();
		PMDoc.Save();
		base.OnFormClosing(e);
	}

	private void ExportFileMenuItem_Click(object sender, EventArgs e)
	{
		if (ExportSettingsDialog.ShowDialog() == DialogResult.OK)
		{
			PMDoc.Save(ExportSettingsDialog.FileName);
		}
	}

	private void RevertFileMenuItem_Click(object sender, EventArgs e)
	{
		PMDoc.Close();
		PowerMateApp.PowerMateDoc = (PowerMateDoc)OriginalPMDoc.Clone();
		PMDoc.AssignPowerMates(PowerMateApp.PowerMates, DeviceAssignment.Default);
	}

	private void CloseFileMenuItem_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void PowerMateMenu_Opening(object sender, CancelEventArgs e)
	{
		int count = PowerMateList.SelectedItems.Count;
		bool flag = count > 0;
		bool flag2 = count == 1;
		bool flag3 = count < PowerMateList.Items.Count;
		ConfigurePowerMateMenuItem.Enabled = flag2;
		RemovePowerMateMenuItem.Enabled = flag;
		RenamePowerMateMenuItem.Enabled = flag2;
		SwitchPowerMateMenuItem.Enabled = flag2 && flag3;
		ReplacePowerMateMenuItem.Enabled = flag && flag3;
		DuplicatePowerMateMenuItem.Enabled = flag;
		ExportPowerMateMenuItem.Enabled = flag;
		UpdatePowerMateMenuItemDropDown(SwitchPowerMateMenuItem.DropDown);
		UpdatePowerMateMenuItemDropDown(ReplacePowerMateMenuItem.DropDown);
	}

	private void ConfigureMenuItem_Click(object sender, EventArgs e)
	{
		PMConfigure.Show(((DeviceColumnItem)PowerMateList.SelectedItems[0]).Node, this);
	}

	private void AddPowerMate_Click(object sender, EventArgs e)
	{
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
		PowerMateList.AddPowerMate();
	}

	private void RemovePowerMates_Click(object sender, EventArgs e)
	{
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
		PowerMateList.RemoveSelected();
	}

	private void RenamePowerMate_Click(object sender, EventArgs e)
	{
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
		if (PowerMateList.CurrentIndex >= 0)
		{
			PowerMateList.ChangeItemText(PowerMateList.CurrentIndex);
		}
	}

	private void InitializePowerMateMenus()
	{
		PowerMateList.Items.ItemAdded += PowerMates_ItemAdded;
		PowerMateList.Items.ItemRemoved += PowerMates_ItemRemoved;
		SwitchPowerMateMenuItem.DropDown.PerformLayout();
		ReplacePowerMateMenuItem.DropDown.PerformLayout();
	}

	private void PowerMates_ItemAdded(ColumnItemCollection sender, IColumnItem item, int index)
	{
		_ = SwitchPowerMateMenuItem.DropDownItems.Count;
		_ = ReplacePowerMateMenuItem.DropDownItems.Count;
		SwitchPowerMateMenuItem.DropDownItems.Insert(index, new ColumnItemMenuItem(item));
		ReplacePowerMateMenuItem.DropDownItems.Insert(index, new ColumnItemMenuItem(item));
	}

	private void PowerMates_ItemRemoved(ColumnItemCollection sender, IColumnItem item, int index)
	{
		SwitchPowerMateMenuItem.DropDownItems.RemoveAt(index);
		ReplacePowerMateMenuItem.DropDownItems.RemoveAt(index);
	}

	private void UpdatePowerMateMenuItemDropDown(ToolStripDropDown dropDown)
	{
		for (int i = 0; i < dropDown.Items.Count; i++)
		{
			dropDown.Items[i].Visible = !PowerMateList.Items[i].Selected;
		}
	}

	private void SwitchPowerMateMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
		int currentIndex = PowerMateList.CurrentIndex;
		PowerMateList.SwitchSettings(currentIndex, PowerMateList.Items.IndexOf(((ColumnItemMenuItem)e.ClickedItem).ColumnItem));
		PowerMateList.SelectAdd(currentIndex);
	}

	private void ReplacePowerMateMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
		int currentIndex = PowerMateList.CurrentIndex;
		PowerMateList.ReplaceSelected((DeviceColumnItem)((ColumnItemMenuItem)e.ClickedItem).ColumnItem);
		PowerMateList.SelectAdd(currentIndex);
	}

	private void DuplicatePowerMateMenuItem_Click(object sender, EventArgs e)
	{
		PowerMateList.DuplicateSelected();
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
	}

	private void ImportPowerMateMenuItem_Click(object sender, EventArgs e)
	{
		if (PowerMateList.ImportSettings() > 0 && PowerMatesHidden)
		{
			ShowPowerMates();
		}
	}

	private void ExportPowerMateMenuItem_Click(object sender, EventArgs e)
	{
		PowerMateList.ExportSelected();
	}

	private void ApplicationMenu_Opening(object sender, CancelEventArgs e)
	{
		int count = ApplicationList.SelectedItems.Count;
		bool enabled = count > 0;
		bool enabled2 = count == 1;
		RemoveApplicationMenuItem.Enabled = enabled;
		RenameApplicationMenuItem.Enabled = enabled2;
		CopyApplicationMenuItem.Enabled = enabled;
		ChangeIconApplicationMenuItem.Enabled = enabled2;
	}

	private void DefaultApplicationMenuItem_Click(object sender, EventArgs e)
	{
		if (DefaultApps.Visible)
		{
			DefaultApps.BringToFront();
		}
		else
		{
			DefaultApps.Show(this);
		}
	}

	private void AddApplication_Click(object sender, EventArgs e)
	{
		ApplicationList.AddApplication();
	}

	private void RemoveApplications_Click(object sender, EventArgs e)
	{
		ApplicationList.RemoveSelectedApplications();
	}

	private void RenameApplication_Click(object sender, EventArgs e)
	{
		if (ApplicationList.CurrentIndex >= 0)
		{
			ApplicationList.ChangeItemText(ApplicationList.CurrentIndex);
		}
	}

	private void CopyApplicationMenuItem_Click(object sender, EventArgs e)
	{
		ColumnItemCollection selectedItems = ApplicationList.SelectedItems;
		CopiedAppNodes = new AppNode[selectedItems.Count];
		for (int i = 0; i < CopiedAppNodes.Length; i++)
		{
			CopiedAppNodes[i] = ((ApplicationColumnItem)selectedItems[i]).Node;
		}
		PasteApplicationMenuItem.Enabled = true;
	}

	private void PasteApplicationMenuItem_Click(object sender, EventArgs e)
	{
		if (CopiedAppNodes != null)
		{
			AppNode[] copiedAppNodes = CopiedAppNodes;
			foreach (AppNode appNode in copiedAppNodes)
			{
				ApplicationList.AddApplication((AppNode)appNode.Clone(), ApplicationList.Items.Count);
			}
		}
	}

	private void ChangeIconApplicationMenuItem_Click(object sender, EventArgs e)
	{
		if (ApplicationList.CurrentIndex >= 0)
		{
			ApplicationColumnItem applicationColumnItem = (ApplicationColumnItem)ApplicationList.Items[ApplicationList.CurrentIndex];
			SelectIconDialog selectIconDialog = new SelectIconDialog();
			selectIconDialog.IconFile = applicationColumnItem.IconFile;
			if (selectIconDialog.ShowDialog() == DialogResult.OK)
			{
				applicationColumnItem.IconPath = selectIconDialog.IconFile + "," + selectIconDialog.SelectedIconIndex;
			}
		}
	}

	private void ViewManualMenuItem_Click(object sender, EventArgs e)
	{
		Process process = new Process();
		process.StartInfo.FileName = Application.StartupPath + "\\" + ManualFilename;
		process.StartInfo.UseShellExecute = true;
		try
		{
			process.Start();
		}
		catch
		{
		}
	}

	private void AboutPowerMateMenuItem_Click(object sender, EventArgs e)
	{
		if (AboutPowerMateForm != null && !AboutPowerMateForm.Visible)
		{
			AboutPowerMateForm = null;
		}
		if (AboutPowerMateForm == null)
		{
			AboutPowerMateForm = new AboutPowerMate();
			AboutPowerMateForm.Show();
		}
		else
		{
			AboutPowerMateForm.WindowState = FormWindowState.Normal;
			AboutPowerMateForm.Activate();
		}
	}

	private void HidePowerMatesButton_Click(object sender, EventArgs e)
	{
		if (PowerMatesHidden)
		{
			ShowPowerMates();
		}
		else
		{
			HidePowerMates();
		}
	}

	private void ShowPowerMates()
	{
		PowerMatesHidden = false;
		HidePowerMatesButton.Image = HidePowerMatesImage;
		PowerMateList.Height = PMStatusStrip.Top - ActionList.Bottom + 2;
		PowerMateList.Anchor |= AnchorStyles.Top;
		ApplicationList.Anchor &= ~(ApplicationList.Anchor & AnchorStyles.Bottom);
		PowerMateList.Enabled = true;
		HidePowerMatesTimer.Start();
	}

	private void HidePowerMates()
	{
		PowerMatesHidden = true;
		HidePowerMatesButton.Image = ShowPowermatesImage;
		PowerMateList.Anchor &= ~(PowerMateList.Anchor & AnchorStyles.Top);
		ApplicationList.Anchor |= AnchorStyles.Bottom;
		PowerMateList.Enabled = false;
		HidePowerMatesTimer.Start();
	}

	private void HidePowerMatesTimer_Tick(object sender, EventArgs e)
	{
		if (PowerMatesHidden)
		{
			if (PowerMateList.Top + PMPixelChange < PMStatusStrip.Top)
			{
				ApplicationList.Height += PMPixelChange;
				PowerMateList.Top += PMPixelChange;
			}
			else
			{
				HidePowerMatesTimer.Stop();
				PowerMateList.Top = PMStatusStrip.Top;
				ApplicationList.Height = PMStatusStrip.Top - ApplicationList.Top + 1;
			}
		}
		else if (PowerMateList.Location.Y - PMPixelChange > ActionSettings.Top)
		{
			ApplicationList.Height -= PMPixelChange;
			PowerMateList.Top -= PMPixelChange;
		}
		else
		{
			HidePowerMatesTimer.Stop();
			ApplicationList.Height = ActionList.Height;
			PowerMateList.Top = ActionSettings.Top - 1;
		}
	}
}
