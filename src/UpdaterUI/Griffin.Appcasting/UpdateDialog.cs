using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Griffin.Appcasting;

public class UpdateDialog : Form
{
	private IContainer components;

	private Label TitleLabel;

	private Label TextLabel;

	private Label ReleaseNotesLabel;

	private RichTextBox ReleaseNotesText;

	private Button SkipButton;

	private Button RemindLaterButton;

	private Button InstallButton;

	private PictureBox ImageBox;

	private UpdateDialogResult _DialogResult;

	private AppcastItem _Item;

	private string _ApplicationName;

	private string _CurrentVersion;

	private string _CurrentBuild;

	private bool _ShowReleaseNotes = true;

	public new UpdateDialogResult DialogResult => _DialogResult;

	public AppcastItem Item
	{
		get
		{
			return _Item;
		}
		set
		{
			_Item = value;
			if (value != null && !string.IsNullOrEmpty(value.Description))
			{
				if (!string.IsNullOrEmpty(value.Title))
				{
					ReleaseNotesText.AppendText(value.Title);
					ReleaseNotesText.SelectAll();
					ReleaseNotesText.SelectionFont = new Font(ReleaseNotesText.Font, FontStyle.Bold);
					ReleaseNotesText.DeselectAll();
					ReleaseNotesText.AppendText(Environment.NewLine + Environment.NewLine);
				}
				ReleaseNotesText.AppendText(value.Description);
				ReleaseNotesText.SelectionStart = 0;
				ShowReleaseNotes = true;
			}
			else
			{
				ReleaseNotesText.Clear();
				ShowReleaseNotes = false;
			}
			SetText();
		}
	}

	public string CurrentVersion
	{
		get
		{
			return _CurrentVersion;
		}
		set
		{
			_CurrentVersion = value;
			SetText();
		}
	}

	public string CurrentBuild
	{
		get
		{
			return _CurrentBuild;
		}
		set
		{
			_CurrentBuild = value;
			SetText();
		}
	}

	public string ApplicationName
	{
		get
		{
			return _ApplicationName;
		}
		set
		{
			_ApplicationName = value;
			SetTitle();
			SetText();
		}
	}

	public string Caption
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	public Image Image
	{
		get
		{
			return ImageBox.Image;
		}
		set
		{
			ImageBox.Image = value;
		}
	}

	private bool ShowReleaseNotes
	{
		get
		{
			return _ShowReleaseNotes;
		}
		set
		{
			if (value != _ShowReleaseNotes)
			{
				Label releaseNotesLabel = ReleaseNotesLabel;
				bool visible = (ReleaseNotesText.Visible = value);
				releaseNotesLabel.Visible = visible;
				int num = ReleaseNotesLabel.Height + ReleaseNotesText.Height;
				if (value)
				{
					base.Height += num;
				}
				else
				{
					base.Height -= num;
				}
			}
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
		this.TitleLabel = new System.Windows.Forms.Label();
		this.TextLabel = new System.Windows.Forms.Label();
		this.ReleaseNotesLabel = new System.Windows.Forms.Label();
		this.ReleaseNotesText = new System.Windows.Forms.RichTextBox();
		this.SkipButton = new System.Windows.Forms.Button();
		this.RemindLaterButton = new System.Windows.Forms.Button();
		this.InstallButton = new System.Windows.Forms.Button();
		this.ImageBox = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.ImageBox).BeginInit();
		base.SuspendLayout();
		this.TitleLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TitleLabel.Location = new System.Drawing.Point(90, 9);
		this.TitleLabel.Name = "TitleLabel";
		this.TitleLabel.Size = new System.Drawing.Size(377, 19);
		this.TitleLabel.TabIndex = 0;
		this.TitleLabel.Text = "A new version is available!";
		this.TextLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TextLabel.Location = new System.Drawing.Point(90, 28);
		this.TextLabel.Name = "TextLabel";
		this.TextLabel.Size = new System.Drawing.Size(377, 29);
		this.TextLabel.TabIndex = 1;
		this.TextLabel.Text = "An update is now available. Would you like to download it now?";
		this.ReleaseNotesLabel.AutoSize = true;
		this.ReleaseNotesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ReleaseNotesLabel.Location = new System.Drawing.Point(91, 57);
		this.ReleaseNotesLabel.Name = "ReleaseNotesLabel";
		this.ReleaseNotesLabel.Size = new System.Drawing.Size(83, 12);
		this.ReleaseNotesLabel.TabIndex = 2;
		this.ReleaseNotesLabel.Text = "Release Notes:";
		this.ReleaseNotesText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ReleaseNotesText.BackColor = System.Drawing.SystemColors.Window;
		this.ReleaseNotesText.Location = new System.Drawing.Point(93, 72);
		this.ReleaseNotesText.Name = "ReleaseNotesText";
		this.ReleaseNotesText.ReadOnly = true;
		this.ReleaseNotesText.Size = new System.Drawing.Size(374, 214);
		this.ReleaseNotesText.TabIndex = 3;
		this.ReleaseNotesText.Text = "";
		this.SkipButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.SkipButton.AutoSize = true;
		this.SkipButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
		this.SkipButton.Location = new System.Drawing.Point(93, 295);
		this.SkipButton.Name = "SkipButton";
		this.SkipButton.Size = new System.Drawing.Size(99, 23);
		this.SkipButton.TabIndex = 4;
		this.SkipButton.Text = "Skip This Version";
		this.SkipButton.UseVisualStyleBackColor = true;
		this.SkipButton.Click += new System.EventHandler(SkipButton_Click);
		this.RemindLaterButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.RemindLaterButton.AutoSize = true;
		this.RemindLaterButton.DialogResult = System.Windows.Forms.DialogResult.No;
		this.RemindLaterButton.Location = new System.Drawing.Point(280, 295);
		this.RemindLaterButton.Name = "RemindLaterButton";
		this.RemindLaterButton.Size = new System.Drawing.Size(99, 23);
		this.RemindLaterButton.TabIndex = 5;
		this.RemindLaterButton.Text = "Remind Me Later";
		this.RemindLaterButton.UseVisualStyleBackColor = true;
		this.RemindLaterButton.Click += new System.EventHandler(RemindLaterButton_Click);
		this.InstallButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.InstallButton.AutoSize = true;
		this.InstallButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
		this.InstallButton.Location = new System.Drawing.Point(384, 295);
		this.InstallButton.Name = "InstallButton";
		this.InstallButton.Size = new System.Drawing.Size(83, 23);
		this.InstallButton.TabIndex = 6;
		this.InstallButton.Text = "Install Update";
		this.InstallButton.UseVisualStyleBackColor = true;
		this.InstallButton.Click += new System.EventHandler(InstallButton_Click);
		this.ImageBox.InitialImage = null;
		this.ImageBox.Location = new System.Drawing.Point(15, 9);
		this.ImageBox.Name = "ImageBox";
		this.ImageBox.Size = new System.Drawing.Size(64, 64);
		this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.ImageBox.TabIndex = 7;
		this.ImageBox.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(479, 330);
		base.Controls.Add(this.TitleLabel);
		base.Controls.Add(this.InstallButton);
		base.Controls.Add(this.RemindLaterButton);
		base.Controls.Add(this.SkipButton);
		base.Controls.Add(this.ReleaseNotesText);
		base.Controls.Add(this.ReleaseNotesLabel);
		base.Controls.Add(this.TextLabel);
		base.Controls.Add(this.ImageBox);
		base.MaximizeBox = false;
		this.MinimumSize = new System.Drawing.Size(404, 138);
		base.Name = "UpdateDialog";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Software Update";
		((System.ComponentModel.ISupportInitialize)this.ImageBox).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public UpdateDialog()
		: this(null, null)
	{
	}

	public UpdateDialog(string appname)
		: this(appname, null)
	{
	}

	public UpdateDialog(string appName, AppcastItem item)
		: this(appName, item, null)
	{
	}

	public UpdateDialog(string appName, AppcastItem item, Image image)
	{
		InitializeComponent();
		ApplicationName = appName;
		Item = item;
		Image = image;
	}

	public new UpdateDialogResult ShowDialog()
	{
		base.ShowDialog();
		return DialogResult;
	}

	public new UpdateDialogResult ShowDialog(IWin32Window owner)
	{
		base.ShowDialog(owner);
		return DialogResult;
	}

	private void SetTitle()
	{
		if (string.IsNullOrEmpty(ApplicationName))
		{
			TitleLabel.Text = "A new verions is available!";
		}
		else
		{
			TitleLabel.Text = "A new version of " + ApplicationName + " is available!";
		}
	}

	private void SetText()
	{
		string text = "";
		if (Item != null)
		{
			if (Item.Version != null)
			{
				text = text + Item.Version + " ";
			}
			if (Item.Build != null)
			{
				text = text + "build " + Item.Build + " ";
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = ((!string.IsNullOrEmpty(ApplicationName)) ? ("A " + ApplicationName + " update ") : "An update ");
		}
		else if (!string.IsNullOrEmpty(ApplicationName))
		{
			text = ApplicationName + " " + text;
		}
		text += "is now available";
		if (CurrentVersion != null || CurrentBuild != null)
		{
			text += " (you have";
			if (CurrentVersion != null)
			{
				text = text + " " + CurrentVersion;
			}
			if (CurrentBuild != null)
			{
				text = text + " build " + CurrentBuild;
			}
			text += ")";
		}
		text += ". Would you like to download it now?";
		TextLabel.Text = text;
	}

	private void InstallButton_Click(object sender, EventArgs e)
	{
		_DialogResult = UpdateDialogResult.InstallUpdate;
	}

	private void RemindLaterButton_Click(object sender, EventArgs e)
	{
		_DialogResult = UpdateDialogResult.RemindMeLater;
	}

	private void SkipButton_Click(object sender, EventArgs e)
	{
		_DialogResult = UpdateDialogResult.SkipThisVersion;
	}
}
