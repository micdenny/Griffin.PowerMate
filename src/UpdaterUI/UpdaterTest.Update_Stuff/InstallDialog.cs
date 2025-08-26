using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UpdaterTest.Update_Stuff;

public class InstallDialog : Form
{
	private IContainer components;

	private Button InstallButton;

	public InstallDialog()
	{
		InitializeComponent();
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
		this.InstallButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.InstallButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.InstallButton.AutoSize = true;
		this.InstallButton.Location = new System.Drawing.Point(313, 96);
		this.InstallButton.Name = "InstallButton";
		this.InstallButton.Size = new System.Drawing.Size(102, 23);
		this.InstallButton.TabIndex = 0;
		this.InstallButton.Text = "Install && Relaunch";
		this.InstallButton.UseVisualStyleBackColor = true;
		base.AcceptButton = this.InstallButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(427, 131);
		base.Controls.Add(this.InstallButton);
		base.Name = "InstallDialog";
		this.Text = "InstallDialog";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
