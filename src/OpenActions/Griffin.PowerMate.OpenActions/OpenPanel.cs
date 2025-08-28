using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.OpenActions;

public class OpenPanel : Panel, IPMActionPanel
{
	private IContainer components;

	private Label FileLabel;

	private TextBox FileTextBox;

	private Button BrowseButton;

	private OpenFileDialog BrowseFileDialog;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string[] Settings
	{
		get
		{
			if (!string.IsNullOrEmpty(FileTextBox.Text))
			{
				return new string[1] { FileTextBox.Text };
			}
			return null;
		}
		set
		{
			if (value != null && value.Length > 0)
			{
				FileTextBox.Text = value[0];
			}
			else
			{
				FileTextBox.Text = null;
			}
		}
	}

	public event EventHandler UpdateSettings;

	public OpenPanel()
	{
		InitializeComponent();
	}

	private void FileTextBox_TextChanged(object sender, EventArgs e)
	{
		OnUpdateSettings(e);
	}

	protected virtual void OnUpdateSettings(EventArgs e)
	{
		if (this.UpdateSettings != null)
		{
			this.UpdateSettings(this, e);
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (base.Parent != null)
		{
			base.Size = base.Parent.Size;
		}
		base.OnParentChanged(e);
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		BrowseFileDialog.ShowDialog();
		FileTextBox.Text = "\"" + BrowseFileDialog.FileName + "\"";
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
		this.FileLabel = new System.Windows.Forms.Label();
		this.FileTextBox = new System.Windows.Forms.TextBox();
		this.BrowseButton = new System.Windows.Forms.Button();
		this.BrowseFileDialog = new System.Windows.Forms.OpenFileDialog();
		base.SuspendLayout();
		this.FileLabel.AutoSize = true;
		this.FileLabel.Location = new System.Drawing.Point(3, 5);
		this.FileLabel.Name = "FileLabel";
		this.FileLabel.Size = new System.Drawing.Size(29, 13);
		this.FileLabel.TabIndex = 0;
		this.FileLabel.Text = "File: ";
		this.FileTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FileTextBox.Location = new System.Drawing.Point(36, 2);
		this.FileTextBox.Name = "FileTextBox";
		this.FileTextBox.Size = new System.Drawing.Size(216, 20);
		this.FileTextBox.TabIndex = 1;
		this.FileTextBox.TextChanged += new System.EventHandler(FileTextBox_TextChanged);
		this.BrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BrowseButton.Location = new System.Drawing.Point(258, 0);
		this.BrowseButton.Name = "BrowseButton";
		this.BrowseButton.Size = new System.Drawing.Size(75, 23);
		this.BrowseButton.TabIndex = 2;
		this.BrowseButton.Text = "Browse...";
		this.BrowseButton.UseVisualStyleBackColor = true;
		this.BrowseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.BrowseFileDialog.Filter = "All Files|*.*";
		this.BrowseFileDialog.SupportMultiDottedExtensions = true;
		this.BrowseFileDialog.Title = "Choose File to Open";
		this.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		base.Controls.Add(this.BrowseButton);
		base.Controls.Add(this.FileTextBox);
		base.Controls.Add(this.FileLabel);
		base.Size = new System.Drawing.Size(339, 31);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
