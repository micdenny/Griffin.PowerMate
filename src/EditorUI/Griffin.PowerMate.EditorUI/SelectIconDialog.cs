using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class SelectIconDialog : Form
{
	private IContainer components;

	private Label FileLabel;

	private TextBox FileTextBox;

	private Button BrowseButton;

	private OpenFileDialog SelectIconFileDialog;

	private Button OkButton;

	private Button Cancel_Button;

	private ImageList IconImageList;

	private IconSelector IconSelector;

	public string IconFile
	{
		get
		{
			return SelectIconFileDialog.FileName;
		}
		set
		{
			SelectIconFileDialog.FileName = value;
			FileTextBox.Text = value;
			ReloadIconSelector();
		}
	}

	public Icon SelectedIcon => IconSelector.SelectedIcon;

	public int SelectedIconIndex => IconSelector.SelectedIconIndex;

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (Icon icon in IconSelector.Icons)
			{
				icon.Dispose();
			}
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.FileLabel = new System.Windows.Forms.Label();
		this.FileTextBox = new System.Windows.Forms.TextBox();
		this.BrowseButton = new System.Windows.Forms.Button();
		this.SelectIconFileDialog = new System.Windows.Forms.OpenFileDialog();
		this.OkButton = new System.Windows.Forms.Button();
		this.Cancel_Button = new System.Windows.Forms.Button();
		this.IconImageList = new System.Windows.Forms.ImageList(this.components);
		this.IconSelector = new Griffin.PowerMate.EditorUI.IconSelector();
		base.SuspendLayout();
		this.FileLabel.AutoSize = true;
		this.FileLabel.Location = new System.Drawing.Point(13, 13);
		this.FileLabel.Name = "FileLabel";
		this.FileLabel.Size = new System.Drawing.Size(26, 13);
		this.FileLabel.TabIndex = 1;
		this.FileLabel.Text = "File:";
		this.FileTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FileTextBox.Location = new System.Drawing.Point(45, 10);
		this.FileTextBox.Name = "FileTextBox";
		this.FileTextBox.ReadOnly = true;
		this.FileTextBox.Size = new System.Drawing.Size(278, 20);
		this.FileTextBox.TabIndex = 2;
		this.FileTextBox.TabStop = false;
		this.BrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BrowseButton.Location = new System.Drawing.Point(329, 8);
		this.BrowseButton.Name = "BrowseButton";
		this.BrowseButton.Size = new System.Drawing.Size(75, 23);
		this.BrowseButton.TabIndex = 4;
		this.BrowseButton.Text = "Browse";
		this.BrowseButton.UseVisualStyleBackColor = true;
		this.BrowseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.SelectIconFileDialog.Filter = "Executables|*.exe|Icon files|*.ico|Dynamic-link libraries|*.dll|All files|*.*";
		this.SelectIconFileDialog.SupportMultiDottedExtensions = true;
		this.SelectIconFileDialog.Title = "Select Icon File";
		this.OkButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.OkButton.Location = new System.Drawing.Point(329, 269);
		this.OkButton.Name = "OkButton";
		this.OkButton.Size = new System.Drawing.Size(75, 23);
		this.OkButton.TabIndex = 3;
		this.OkButton.Text = "OK";
		this.OkButton.UseVisualStyleBackColor = true;
		this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.Cancel_Button.Location = new System.Drawing.Point(248, 269);
		this.Cancel_Button.Name = "Cancel_Button";
		this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
		this.Cancel_Button.TabIndex = 2;
		this.Cancel_Button.Text = "Cancel";
		this.Cancel_Button.UseVisualStyleBackColor = true;
		this.IconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
		this.IconImageList.ImageSize = new System.Drawing.Size(32, 32);
		this.IconImageList.TransparentColor = System.Drawing.Color.Transparent;
		this.IconSelector.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.IconSelector.AutoScroll = true;
		this.IconSelector.BackColor = System.Drawing.SystemColors.Window;
		this.IconSelector.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.IconSelector.IconPadding = 3;
		this.IconSelector.IconSize = 32;
		this.IconSelector.Location = new System.Drawing.Point(12, 37);
		this.IconSelector.Name = "IconSelector";
		this.IconSelector.SelectedIcon = null;
		this.IconSelector.SelectedIconIndex = -1;
		this.IconSelector.Size = new System.Drawing.Size(392, 226);
		this.IconSelector.TabIndex = 1;
		base.AcceptButton = this.OkButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.Cancel_Button;
		base.ClientSize = new System.Drawing.Size(416, 304);
		base.Controls.Add(this.IconSelector);
		base.Controls.Add(this.Cancel_Button);
		base.Controls.Add(this.OkButton);
		base.Controls.Add(this.BrowseButton);
		base.Controls.Add(this.FileTextBox);
		base.Controls.Add(this.FileLabel);
		this.MinimumSize = new System.Drawing.Size(188, 192);
		base.Name = "SelectIconDialog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Select an Icon";
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public SelectIconDialog()
	{
		InitializeComponent();
	}

	public new void Show()
	{
		base.DialogResult = ChangeIconFile(null);
		if (base.DialogResult == DialogResult.OK)
		{
			base.Show();
		}
	}

	public new void Show(IWin32Window owner)
	{
		base.DialogResult = ChangeIconFile(owner);
		if (base.DialogResult == DialogResult.OK)
		{
			base.Show(owner);
		}
	}

	public new DialogResult ShowDialog()
	{
		base.DialogResult = ChangeIconFile(null);
		if (base.DialogResult == DialogResult.OK)
		{
			return base.ShowDialog();
		}
		return base.DialogResult;
	}

	public new DialogResult ShowDialog(IWin32Window owner)
	{
		base.DialogResult = ChangeIconFile(owner);
		if (base.DialogResult == DialogResult.OK)
		{
			return base.ShowDialog(owner);
		}
		return base.DialogResult;
	}

	private DialogResult ChangeIconFile(IWin32Window owner)
	{
		DialogResult dialogResult = DialogResult.None;
		dialogResult = ((owner != null) ? SelectIconFileDialog.ShowDialog(owner) : SelectIconFileDialog.ShowDialog());
		if (dialogResult == DialogResult.OK)
		{
			FileTextBox.Text = IconFile;
			ReloadIconSelector();
		}
		return dialogResult;
	}

	private void ReloadIconSelector()
	{
		foreach (Icon icon in IconSelector.Icons)
		{
			icon.Dispose();
		}
		IconSelector.Icons.Clear();
		Icon[] iconsFromFile = IconHelper.GetIconsFromFile(IconFile, IconSize.Large);
		foreach (Icon item in iconsFromFile)
		{
			IconSelector.Icons.Add(item);
		}
		if (IconSelector.Icons.Count > 0)
		{
			IconSelector.SelectedIconIndex = 0;
		}
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		ChangeIconFile(base.Owner);
	}
}
