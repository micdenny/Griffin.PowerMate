using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

public class AboutPowerMate : Form
{
	private IContainer components;

	private Label PowerMateLabel;

	private Label VersionLabel;

	private Label BuildLabel;

	private RichTextBox AboutTextBox;

	private PictureBox pictureBox1;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Griffin.PowerMate.EditorUI.AboutPowerMate));
		this.PowerMateLabel = new System.Windows.Forms.Label();
		this.VersionLabel = new System.Windows.Forms.Label();
		this.BuildLabel = new System.Windows.Forms.Label();
		this.AboutTextBox = new System.Windows.Forms.RichTextBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.PowerMateLabel.AutoSize = true;
		this.PowerMateLabel.BackColor = System.Drawing.Color.Transparent;
		this.PowerMateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PowerMateLabel.Location = new System.Drawing.Point(340, 9);
		this.PowerMateLabel.Name = "PowerMateLabel";
		this.PowerMateLabel.Size = new System.Drawing.Size(98, 20);
		this.PowerMateLabel.TabIndex = 1;
		this.PowerMateLabel.Text = "PowerMate";
		this.VersionLabel.AutoSize = true;
		this.VersionLabel.BackColor = System.Drawing.Color.Transparent;
		this.VersionLabel.Location = new System.Drawing.Point(350, 29);
		this.VersionLabel.Name = "VersionLabel";
		this.VersionLabel.Size = new System.Drawing.Size(48, 13);
		this.VersionLabel.TabIndex = 2;
		this.VersionLabel.Text = "Version: ";
		this.BuildLabel.AutoSize = true;
		this.BuildLabel.BackColor = System.Drawing.Color.Transparent;
		this.BuildLabel.Location = new System.Drawing.Point(362, 45);
		this.BuildLabel.Name = "BuildLabel";
		this.BuildLabel.Size = new System.Drawing.Size(36, 13);
		this.BuildLabel.TabIndex = 3;
		this.BuildLabel.Text = "Build: ";
		this.AboutTextBox.BackColor = System.Drawing.Color.White;
		this.AboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.AboutTextBox.Location = new System.Drawing.Point(299, 72);
		this.AboutTextBox.Name = "AboutTextBox";
		this.AboutTextBox.ReadOnly = true;
		this.AboutTextBox.Size = new System.Drawing.Size(189, 106);
		this.AboutTextBox.TabIndex = 4;
		this.AboutTextBox.Text = "";
		this.AboutTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(AboutTextBox_LinkClicked);
		this.pictureBox1.Image = Griffin.PowerMate.EditorUI.Properties.Resources.powerMateHelp;
		this.pictureBox1.Location = new System.Drawing.Point(-4, 12);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(306, 155);
		this.pictureBox1.TabIndex = 5;
		this.pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(499, 190);
		base.Controls.Add(this.AboutTextBox);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.BuildLabel);
		base.Controls.Add(this.VersionLabel);
		base.Controls.Add(this.PowerMateLabel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "AboutPowerMate";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "About PowerMate";
		base.Load += new System.EventHandler(AboutPowerMate_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public AboutPowerMate()
	{
		InitializeComponent();
	}

	private void AboutPowerMate_Load(object sender, EventArgs e)
	{
		VersionLabel.Text += PowerMateApp.Version;
		BuildLabel.Text += PowerMateApp.Build;
		AboutTextBox.Rtf = Resources.credits;
	}

	private void AboutTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		Process process = new Process();
		process.StartInfo.FileName = e.LinkText;
		process.StartInfo.UseShellExecute = true;
		process.Start();
	}
}
