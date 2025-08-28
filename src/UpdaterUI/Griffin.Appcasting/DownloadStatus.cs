using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Griffin.Appcasting;

public class DownloadStatus : Form
{
	private IContainer components;

	private ProgressBar DownloadProgressBar;

	private Button ActionButton;

	private Label AmountDownloadedLabel;

	private Label DownloadingLabel;

	private Label TimeLabel;

	private PictureBox ImageBox;

	private string _ApplicationName;

	private DateTime DownloadStartTime;

	private DateTime LastDownloadRateTime;

	private long LastDownloadRateBytes;

	private long DownloadRate;

	private AppcastUpdater _AppcastUpdater;

	private DownloadCompletePrompt _DownloadCompletePrompt;

	private bool IsDownloadComplete;

	private AppcastUpdaterDownloadEventArgs DownloadingArgs;

	private DownloadProgressChangedEventHandler DownloadProgressChangedHandler;

	private EventHandler<AppcastUpdaterDownloadEventArgs> DownloadStartedHandler;

	private EventHandler<AppcastUpdaterDownloadEventArgs> DownloadCompletedHandler;

	private EventHandler<AppcastUpdaterDownloadEventArgs> DownloadCancelledHandler;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public AppcastUpdater AppcastUpdater
	{
		get
		{
			return _AppcastUpdater;
		}
		set
		{
			if (_AppcastUpdater != null)
			{
				_AppcastUpdater.DownloadProgressChanged -= DownloadProgressChangedHandler;
				_AppcastUpdater.DownloadStarted -= DownloadStartedHandler;
				_AppcastUpdater.DonwloadCompleted -= DownloadCompletedHandler;
				_AppcastUpdater.DownloadCancelled -= DownloadCancelledHandler;
			}
			if (value != null)
			{
				value.DownloadProgressChanged += DownloadProgressChangedHandler;
				value.DownloadStarted += DownloadStartedHandler;
				value.DonwloadCompleted += DownloadCompletedHandler;
				value.DownloadCancelled += DownloadCancelledHandler;
				ActionButton.Enabled = true;
			}
			else
			{
				ActionButton.Enabled = false;
			}
			_AppcastUpdater = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ApplicationName
	{
		get
		{
			return _ApplicationName;
		}
		set
		{
			_ApplicationName = value;
			Text = "Updating " + value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DownloadCompletePrompt DownloadCompletePrompt
	{
		get
		{
			return _DownloadCompletePrompt;
		}
		set
		{
			_DownloadCompletePrompt = value;
		}
	}

	public event EventHandler UserCancelled;

	public event EventHandler<AppcastUpdaterDownloadEventArgs> InstallClick;

	public event EventHandler<AppcastUpdaterDownloadEventArgs> InstallAndRelaunchClick;

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
		this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
		this.ActionButton = new System.Windows.Forms.Button();
		this.AmountDownloadedLabel = new System.Windows.Forms.Label();
		this.DownloadingLabel = new System.Windows.Forms.Label();
		this.TimeLabel = new System.Windows.Forms.Label();
		this.ImageBox = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.ImageBox).BeginInit();
		base.SuspendLayout();
		this.DownloadProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DownloadProgressBar.Location = new System.Drawing.Point(85, 48);
		this.DownloadProgressBar.Name = "DownloadProgressBar";
		this.DownloadProgressBar.Size = new System.Drawing.Size(286, 23);
		this.DownloadProgressBar.TabIndex = 0;
		this.ActionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ActionButton.AutoSize = true;
		this.ActionButton.Enabled = false;
		this.ActionButton.Location = new System.Drawing.Point(295, 77);
		this.ActionButton.Name = "ActionButton";
		this.ActionButton.Size = new System.Drawing.Size(76, 23);
		this.ActionButton.TabIndex = 1;
		this.ActionButton.Text = "Cancel";
		this.ActionButton.UseVisualStyleBackColor = true;
		this.ActionButton.Click += new System.EventHandler(ActionButton_Click);
		this.AmountDownloadedLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.AmountDownloadedLabel.AutoEllipsis = true;
		this.AmountDownloadedLabel.Location = new System.Drawing.Point(96, 82);
		this.AmountDownloadedLabel.Name = "AmountDownloadedLabel";
		this.AmountDownloadedLabel.Size = new System.Drawing.Size(193, 13);
		this.AmountDownloadedLabel.TabIndex = 2;
		this.AmountDownloadedLabel.Text = "0 KB of 0 KB";
		this.AmountDownloadedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.DownloadingLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.DownloadingLabel.AutoEllipsis = true;
		this.DownloadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DownloadingLabel.Location = new System.Drawing.Point(85, 9);
		this.DownloadingLabel.Name = "DownloadingLabel";
		this.DownloadingLabel.Size = new System.Drawing.Size(286, 13);
		this.DownloadingLabel.TabIndex = 3;
		this.DownloadingLabel.Text = "Downloading update...";
		this.TimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TimeLabel.Location = new System.Drawing.Point(85, 22);
		this.TimeLabel.Name = "TimeLabel";
		this.TimeLabel.Size = new System.Drawing.Size(286, 23);
		this.TimeLabel.TabIndex = 5;
		this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ImageBox.Location = new System.Drawing.Point(12, 9);
		this.ImageBox.Name = "ImageBox";
		this.ImageBox.Size = new System.Drawing.Size(64, 64);
		this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.ImageBox.TabIndex = 6;
		this.ImageBox.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(383, 110);
		base.Controls.Add(this.ImageBox);
		base.Controls.Add(this.ActionButton);
		base.Controls.Add(this.TimeLabel);
		base.Controls.Add(this.DownloadingLabel);
		base.Controls.Add(this.AmountDownloadedLabel);
		base.Controls.Add(this.DownloadProgressBar);
		base.MaximizeBox = false;
		base.Name = "DownloadStatus";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Updating";
		((System.ComponentModel.ISupportInitialize)this.ImageBox).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public DownloadStatus()
	{
		InitializeComponent();
		DownloadProgressChangedHandler = DownloadProgressChanged;
		DownloadStartedHandler = DownloadStarted;
		DownloadCompletedHandler = DownloadCompleted;
		DownloadCancelledHandler = DownloadCancelled;
	}

	public DownloadStatus(AppcastUpdater appcastUpdater)
		: this()
	{
		AppcastUpdater = appcastUpdater;
	}

	public DownloadStatus(AppcastUpdater appcastUpdater, string applicationName)
		: this(appcastUpdater)
	{
		ApplicationName = applicationName;
	}

	public DownloadStatus(AppcastUpdater appcastUpdater, string applicationName, Image image)
		: this(appcastUpdater, applicationName)
	{
		Image = image;
	}

	public void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		DownloadProgressBar.Value = e.ProgressPercentage;
		AmountDownloadedLabel.Text = FormatBytes(e.BytesReceived, 1) + " of " + FormatBytes(e.TotalBytesToReceive, 1);
		long num = (long)(DateTime.Now - LastDownloadRateTime).TotalSeconds;
		if (num > 0 && num >= 1)
		{
			DownloadRate = (e.BytesReceived - LastDownloadRateBytes) / num;
			LastDownloadRateTime = DateTime.Now;
			LastDownloadRateBytes = e.BytesReceived;
		}
		Label amountDownloadedLabel = AmountDownloadedLabel;
		amountDownloadedLabel.Text = amountDownloadedLabel.Text + " (" + FormatBytes(DownloadRate, 0) + "/sec)";
		num = (long)(DateTime.Now - DownloadStartTime).TotalSeconds;
		if (num <= 0)
		{
			return;
		}
		long num2 = e.BytesReceived / num;
		if (num2 > 0)
		{
			long num3 = (e.TotalBytesToReceive - e.BytesReceived) / (num2 * 60);
			if (num3 > 60)
			{
				TimeLabel.Text = num3 / 60 + " hours and " + num3 % 60 + " minutes remaining";
			}
			else if (num3 > 1)
			{
				TimeLabel.Text = num3 + " minutes remaining";
			}
			else if (num3 > 0)
			{
				TimeLabel.Text = num3 + " minute remaining";
			}
			else
			{
				TimeLabel.Text = "Less than 1 minute remaining";
			}
		}
	}

	public void DownloadStarted(object sender, AppcastUpdaterDownloadEventArgs e)
	{
		IsDownloadComplete = false;
		LastDownloadRateTime = DateTime.Now;
		DownloadStartTime = DateTime.Now;
		DownloadingLabel.Text = "Downloading Update...";
		ActionButton.Text = "Cancel";
		if (string.IsNullOrEmpty(ApplicationName))
		{
			ApplicationName = e.Item.Title;
			if (string.IsNullOrEmpty(ApplicationName))
			{
				int num = e.Item.Location.LastIndexOf('/') + 1;
				if (num >= 0)
				{
					ApplicationName = e.Item.Location.Substring(num, e.Item.Location.Length - num);
				}
				else
				{
					ApplicationName = e.Item.Location;
				}
			}
		}
		DownloadingArgs = e;
		Show();
	}

	public void DownloadCompleted(object sender, AppcastUpdaterDownloadEventArgs e)
	{
		IsDownloadComplete = true;
		TimeLabel.Text = null;
		AmountDownloadedLabel.Text = null;
		switch (DownloadCompletePrompt)
		{
		case DownloadCompletePrompt.None:
			Close();
			break;
		case DownloadCompletePrompt.Close:
			DownloadingLabel.Text = "Download Completed";
			ActionButton.Text = "Close";
			ActionButton.Enabled = true;
			break;
		case DownloadCompletePrompt.Install:
			DownloadingLabel.Text = "Ready to Install!";
			ActionButton.Text = "Install";
			break;
		case DownloadCompletePrompt.InstallAndRelaunch:
			DownloadingLabel.Text = "Ready to Install!";
			ActionButton.Text = "Install and Relaunch";
			break;
		}
	}

	public void DownloadCancelled(object sender, AppcastUpdaterDownloadEventArgs e)
	{
		if (DownloadCompletePrompt == DownloadCompletePrompt.None)
		{
			Close();
			return;
		}
		DownloadingLabel.Text = "Download Cancelled";
		ActionButton.Text = "Close";
		ActionButton.Enabled = true;
	}

	public static string FormatBytes(long fileSize, int decNum)
	{
		float num = fileSize;
		int num2 = 0;
		while (num > 1024f)
		{
			num /= 1024f;
			num2++;
		}
		string text = num.ToString();
		int num3 = text.IndexOf('.');
		if (num3 < 0)
		{
			num3 = text.Length;
		}
		else if (decNum > 0)
		{
			num3 += decNum + 1;
		}
		if (text.Length > num3)
		{
			text = text.Substring(0, num3);
		}
		return num2 switch
		{
			0 => text + " bytes", 
			1 => text + " KB", 
			2 => text + " MB", 
			3 => text + " GB", 
			4 => text + " TB", 
			_ => text + " (?)", 
		};
	}

	protected virtual void OnUserCancelled(EventArgs e)
	{
		if (this.UserCancelled != null)
		{
			this.UserCancelled(this, e);
		}
	}

	protected virtual void OnInstallClick(AppcastUpdaterDownloadEventArgs e)
	{
		if (this.InstallClick != null)
		{
			this.InstallClick(this, e);
		}
	}

	protected virtual void OnInstallAndRelaunchClick(AppcastUpdaterDownloadEventArgs e)
	{
		if (this.InstallAndRelaunchClick != null)
		{
			this.InstallAndRelaunchClick(this, e);
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);
		if (!e.Cancel && AppcastUpdater != null && AppcastUpdater.Downloading)
		{
			DialogResult dialogResult = MessageBox.Show("Closing this window will cancel the current download.", "Cancel Download", MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.OK)
			{
				AppcastUpdater.CancelDownload();
			}
			else
			{
				e.Cancel = true;
			}
		}
		if (!e.Cancel)
		{
			AppcastUpdater = null;
		}
	}

	private void ActionButton_Click(object sender, EventArgs e)
	{
		if (AppcastUpdater != null && AppcastUpdater.Downloading)
		{
			AppcastUpdater.CancelDownload();
			OnUserCancelled(e);
			return;
		}
		if (IsDownloadComplete)
		{
			if (DownloadCompletePrompt == DownloadCompletePrompt.Install)
			{
				OnInstallClick(DownloadingArgs);
			}
			else if (DownloadCompletePrompt == DownloadCompletePrompt.InstallAndRelaunch)
			{
				OnInstallAndRelaunchClick(DownloadingArgs);
			}
		}
		Close();
	}
}
