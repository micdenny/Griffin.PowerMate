using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.SendKeysActions.Properties;

namespace Griffin.PowerMate.SendKeysActions;

public class SendKeysPanel : Panel, IPMActionPanel
{
	private IContainer components;

	private Label KeysLabel;

	private SendKeysTextBox KeysToSendTextBox;

	private Button UndoKeysButton;

	private string UndoKeysString;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string[] Settings
	{
		get
		{
			if (!string.IsNullOrEmpty(KeysToSendTextBox.Text))
			{
				return new string[1] { KeysToSendTextBox.Text };
			}
			return null;
		}
		set
		{
			if (value != null && value.Length > 0)
			{
				KeysToSendTextBox.Text = value[0];
			}
			else
			{
				KeysToSendTextBox.Text = null;
			}
			UndoKeysString = null;
			UndoKeysButton.Visible = false;
		}
	}

	public event EventHandler UpdateSettings;

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
		this.KeysLabel = new System.Windows.Forms.Label();
		this.UndoKeysButton = new System.Windows.Forms.Button();
		this.KeysToSendTextBox = new Griffin.PowerMate.SendKeysActions.SendKeysTextBox();
		base.SuspendLayout();
		this.KeysLabel.AutoSize = true;
		this.KeysLabel.Location = new System.Drawing.Point(3, 6);
		this.KeysLabel.Name = "KeysLabel";
		this.KeysLabel.Size = new System.Drawing.Size(33, 13);
		this.KeysLabel.TabIndex = 2;
		this.KeysLabel.Text = "Keys:";
		this.UndoKeysButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.UndoKeysButton.Image = Griffin.PowerMate.SendKeysActions.Properties.Resources.cancelKeystrokeBlack;
		this.UndoKeysButton.Location = new System.Drawing.Point(230, 1);
		this.UndoKeysButton.Name = "UndoKeysButton";
		this.UndoKeysButton.Size = new System.Drawing.Size(20, 23);
		this.UndoKeysButton.TabIndex = 4;
		this.UndoKeysButton.UseVisualStyleBackColor = true;
		this.UndoKeysButton.Visible = false;
		this.UndoKeysButton.Click += new System.EventHandler(UndoKeysButton_Click);
		this.KeysToSendTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.KeysToSendTextBox.Location = new System.Drawing.Point(42, 3);
		this.KeysToSendTextBox.Name = "KeysToSendTextBox";
		this.KeysToSendTextBox.Size = new System.Drawing.Size(187, 20);
		this.KeysToSendTextBox.TabIndex = 3;
		this.KeysToSendTextBox.Enter += new System.EventHandler(KeysToSendTextBox_Enter);
		this.KeysToSendTextBox.KeyAdded += new System.EventHandler<Griffin.PowerMate.SendKeysActions.SendKeysEventArgs>(KeysToSendTextBox_KeyAdded);
		this.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		base.Controls.Add(this.UndoKeysButton);
		base.Controls.Add(this.KeysToSendTextBox);
		base.Controls.Add(this.KeysLabel);
		base.Size = new System.Drawing.Size(261, 26);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public SendKeysPanel()
	{
		InitializeComponent();
	}

	public Keys StringToKey(string key)
	{
		return KeysToSendTextBox.StringToKey(key);
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

	private void UndoKeysButton_Click(object sender, EventArgs e)
	{
		if (UndoKeysString != null)
		{
			KeysToSendTextBox.Text = UndoKeysString;
			UndoKeysString = null;
			OnUpdateSettings(EventArgs.Empty);
		}
		UndoKeysButton.Visible = false;
	}

	private void KeysToSendTextBox_Enter(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(KeysToSendTextBox.Text))
		{
			UndoKeysString = KeysToSendTextBox.Text;
			UndoKeysButton.Visible = true;
		}
	}

	private void KeysToSendTextBox_KeyAdded(object sender, EventArgs e)
	{
		OnUpdateSettings(EventArgs.Empty);
	}
}
