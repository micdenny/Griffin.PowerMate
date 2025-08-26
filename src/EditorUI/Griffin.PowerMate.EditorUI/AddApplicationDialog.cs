using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Griffin.PowerMate.EditorUI;

internal class AddApplicationDialog : Form
{
	private string AppPath;

	private IContainer components;

	private TextBox NameTextBox;

	private TextBox ApplicationTextBox;

	private Label NameLabel;

	private Label ApplicationLabel;

	private Button OKButton;

	private Button Cancel_Button;

	private Button BrowseButton;

	private Label SelectOrBrowseLabel;

	private OpenFileDialog SelectFileDialog;

	private RunningAppsHeaderColumn RunningApplications;

	public string ImageName => ApplicationTextBox.Text;

	public string SettingName => NameTextBox.Text;

	public string Path => AppPath;

	public AddApplicationDialog()
	{
		InitializeComponent();
		SelectFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
	}

	public DialogResult ShowDialog(bool includeGlobal)
	{
		RunningApplications.UpdateProcesses(includeGlobal);
		SetAppSelection(null, null, null);
		return base.ShowDialog();
	}

	public void Show(bool includeGlobal)
	{
		RunningApplications.UpdateProcesses(includeGlobal);
		SetAppSelection(null, null, null);
		base.Show();
	}

	private new DialogResult ShowDialog()
	{
		return base.ShowDialog();
	}

	private new DialogResult ShowDialog(IWin32Window owner)
	{
		return base.ShowDialog(owner);
	}

	private new void Show()
	{
		base.Show();
	}

	private new void Show(IWin32Window owner)
	{
		base.Show(owner);
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		if (SelectFileDialog.ShowDialog() == DialogResult.OK)
		{
			RunningApplications.UnSelectAll();
			int num = SelectFileDialog.FileName.LastIndexOf('\\') + 1;
			int length = SelectFileDialog.FileName.LastIndexOf('.') - num;
			string text = SelectFileDialog.FileName.Substring(num, length);
			SetAppSelection(text, text, SelectFileDialog.FileName);
		}
	}

	private void RunningApplications_ItemSelectionChanged(object sender, EventArgs e)
	{
		if (RunningApplications.SelectedItems.Count >= 1)
		{
			RunningAppsColumnItem runningAppsColumnItem = (RunningAppsColumnItem)RunningApplications.SelectedItems[0];
			SetAppSelection(runningAppsColumnItem.Text, runningAppsColumnItem.Image, runningAppsColumnItem.Path);
		}
		else
		{
			SetAppSelection(null, null, null);
		}
	}

	private void SetAppSelection(string name, string app, string path)
	{
		AppPath = path;
		NameTextBox.Text = name;
		ApplicationTextBox.Text = app;
		OKButton.Enabled = name != null || app != null;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Griffin.PowerMate.EditorUI.AddApplicationDialog));
		this.NameTextBox = new System.Windows.Forms.TextBox();
		this.ApplicationTextBox = new System.Windows.Forms.TextBox();
		this.NameLabel = new System.Windows.Forms.Label();
		this.ApplicationLabel = new System.Windows.Forms.Label();
		this.OKButton = new System.Windows.Forms.Button();
		this.Cancel_Button = new System.Windows.Forms.Button();
		this.BrowseButton = new System.Windows.Forms.Button();
		this.SelectOrBrowseLabel = new System.Windows.Forms.Label();
		this.SelectFileDialog = new System.Windows.Forms.OpenFileDialog();
		this.RunningApplications = new Griffin.PowerMate.EditorUI.RunningAppsHeaderColumn();
		base.SuspendLayout();
		this.NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.NameTextBox.Location = new System.Drawing.Point(83, 239);
		this.NameTextBox.Name = "NameTextBox";
		this.NameTextBox.Size = new System.Drawing.Size(139, 20);
		this.NameTextBox.TabIndex = 3;
		this.ApplicationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ApplicationTextBox.Location = new System.Drawing.Point(83, 266);
		this.ApplicationTextBox.Name = "ApplicationTextBox";
		this.ApplicationTextBox.ReadOnly = true;
		this.ApplicationTextBox.Size = new System.Drawing.Size(139, 20);
		this.ApplicationTextBox.TabIndex = 4;
		this.ApplicationTextBox.Text = " ";
		this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.NameLabel.AutoSize = true;
		this.NameLabel.Location = new System.Drawing.Point(12, 242);
		this.NameLabel.Name = "NameLabel";
		this.NameLabel.Size = new System.Drawing.Size(41, 13);
		this.NameLabel.TabIndex = 5;
		this.NameLabel.Text = "Name: ";
		this.ApplicationLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ApplicationLabel.AutoSize = true;
		this.ApplicationLabel.Location = new System.Drawing.Point(12, 269);
		this.ApplicationLabel.Name = "ApplicationLabel";
		this.ApplicationLabel.Size = new System.Drawing.Size(65, 13);
		this.ApplicationLabel.TabIndex = 6;
		this.ApplicationLabel.Text = "Application: ";
		this.OKButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.OKButton.Location = new System.Drawing.Point(238, 237);
		this.OKButton.Name = "OKButton";
		this.OKButton.Size = new System.Drawing.Size(75, 23);
		this.OKButton.TabIndex = 7;
		this.OKButton.Text = "OK";
		this.OKButton.UseVisualStyleBackColor = true;
		this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.Cancel_Button.Location = new System.Drawing.Point(238, 264);
		this.Cancel_Button.Name = "Cancel_Button";
		this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
		this.Cancel_Button.TabIndex = 8;
		this.Cancel_Button.Text = "Cancel";
		this.Cancel_Button.UseVisualStyleBackColor = true;
		this.BrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.BrowseButton.Location = new System.Drawing.Point(218, 7);
		this.BrowseButton.Name = "BrowseButton";
		this.BrowseButton.Size = new System.Drawing.Size(75, 23);
		this.BrowseButton.TabIndex = 9;
		this.BrowseButton.Text = "Browse...";
		this.BrowseButton.UseVisualStyleBackColor = true;
		this.BrowseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.SelectOrBrowseLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.SelectOrBrowseLabel.AutoSize = true;
		this.SelectOrBrowseLabel.Location = new System.Drawing.Point(24, 12);
		this.SelectOrBrowseLabel.Name = "SelectOrBrowseLabel";
		this.SelectOrBrowseLabel.Size = new System.Drawing.Size(193, 13);
		this.SelectOrBrowseLabel.TabIndex = 10;
		this.SelectOrBrowseLabel.Text = "Select a currently running application or";
		this.SelectFileDialog.DefaultExt = "exe";
		this.SelectFileDialog.Filter = "Program Files|*.exe|All Files|*.*";
		this.SelectFileDialog.RestoreDirectory = true;
		this.SelectFileDialog.SupportMultiDottedExtensions = true;
		this.SelectFileDialog.Title = "Add Application";
		this.RunningApplications.AltBackColor = System.Drawing.Color.AliceBlue;
		this.RunningApplications.AlternateRowColor = false;
		this.RunningApplications.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.RunningApplications.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
		this.RunningApplications.BackColor = System.Drawing.SystemColors.Window;
		this.RunningApplications.BorderColor = System.Drawing.SystemColors.HotTrack;
		this.RunningApplications.BorderThickness = 1;
		this.RunningApplications.DrawBorder = true;
		this.RunningApplications.DrawRowLines = false;
		this.RunningApplications.HeaderAlignment = System.Drawing.ContentAlignment.MiddleCenter;
		this.RunningApplications.HeaderColor = System.Drawing.SystemColors.ControlText;
		this.RunningApplications.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RunningApplications.HeaderImage = (System.Drawing.Bitmap)resources.GetObject("RunningApplications.HeaderImage");
		this.RunningApplications.HeaderImageLayout = System.Windows.Forms.ImageLayout.Tile;
		this.RunningApplications.HeaderText = "Currently Running Applications";
		this.RunningApplications.HideSelection = false;
		this.RunningApplications.HideSelectRowColor = System.Drawing.SystemColors.MenuBar;
		this.RunningApplications.IconTextSpace = 3;
		this.RunningApplications.Indent = 3;
		this.RunningApplications.ItemAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.RunningApplications.Location = new System.Drawing.Point(12, 36);
		this.RunningApplications.MultiSelect = false;
		this.RunningApplications.Name = "RunningApplications";
		this.RunningApplications.RowHeight = 16;
		this.RunningApplications.RowLinesColor = System.Drawing.SystemColors.InactiveBorder;
		this.RunningApplications.RowSpacing = 3;
		this.RunningApplications.SelectForeColor = System.Drawing.Color.Transparent;
		this.RunningApplications.SelectionOutline = true;
		this.RunningApplications.SelectRowColor = System.Drawing.SystemColors.MenuHighlight;
		this.RunningApplications.Size = new System.Drawing.Size(301, 192);
		this.RunningApplications.TabIndex = 11;
		this.RunningApplications.TextEdit = false;
		this.RunningApplications.Type = Griffin.PowerMate.EditorUI.ColumnType.IconAndIndentText;
		this.RunningApplications.ItemSelectionChanged += new System.EventHandler(RunningApplications_ItemSelectionChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(325, 294);
		base.Controls.Add(this.RunningApplications);
		base.Controls.Add(this.BrowseButton);
		base.Controls.Add(this.SelectOrBrowseLabel);
		base.Controls.Add(this.Cancel_Button);
		base.Controls.Add(this.OKButton);
		base.Controls.Add(this.ApplicationLabel);
		base.Controls.Add(this.NameLabel);
		base.Controls.Add(this.ApplicationTextBox);
		base.Controls.Add(this.NameTextBox);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(285, 208);
		base.Name = "AddApplicationDialog";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Add Application";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
