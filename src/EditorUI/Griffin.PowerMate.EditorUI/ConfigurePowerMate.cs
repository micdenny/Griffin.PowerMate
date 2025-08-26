using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.EditorUI.Properties;

namespace Griffin.PowerMate.EditorUI;

internal class ConfigurePowerMate : Form
{
	private IContainer components;

	private CheckBox GlobalCheckBox;

	private TrackBar LongClickSlider;

	private Label LongClickLabel;

	private GroupBox LightGroupBox;

	private Label BightnessLabel;

	private Label PulseLabel;

	private TrackBar PulseSlider;

	private TrackBar BrightnessSlider;

	private CheckBox PulseCheckBox;

	private CheckBox SleepCheckBox;

	private Label ZeroSecLabel;

	private Label FiveSecLabel;

	private Label SecondsLabel;

	private Label BrightnessFullLabel;

	private Label BrightnessOffLabel;

	private DeviceNode Device;

	private uint LongClick
	{
		get
		{
			return (uint)LongClickSlider.Value;
		}
		set
		{
			int num = (int)value;
			if (num > LongClickSlider.Maximum)
			{
				num = LongClickSlider.Maximum;
			}
			else if (num < LongClickSlider.Minimum)
			{
				num = LongClickSlider.Minimum;
			}
			LongClickSlider.Value = num;
		}
	}

	private byte PulseRate
	{
		get
		{
			return (byte)PulseSlider.Value;
		}
		set
		{
			int num = value;
			if (num > PulseSlider.Maximum)
			{
				num = PulseSlider.Maximum;
			}
			else if (num < PulseSlider.Minimum)
			{
				num = PulseSlider.Minimum;
			}
			PulseSlider.Value = num;
		}
	}

	private byte Brightness
	{
		get
		{
			return (byte)BrightnessSlider.Value;
		}
		set
		{
			int num = value;
			if (num > BrightnessSlider.Maximum)
			{
				num = BrightnessSlider.Maximum;
			}
			else if (num < BrightnessSlider.Minimum)
			{
				num = BrightnessSlider.Minimum;
			}
			BrightnessSlider.Value = num;
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
		this.GlobalCheckBox = new System.Windows.Forms.CheckBox();
		this.LongClickSlider = new System.Windows.Forms.TrackBar();
		this.LongClickLabel = new System.Windows.Forms.Label();
		this.LightGroupBox = new System.Windows.Forms.GroupBox();
		this.BightnessLabel = new System.Windows.Forms.Label();
		this.PulseSlider = new System.Windows.Forms.TrackBar();
		this.BrightnessSlider = new System.Windows.Forms.TrackBar();
		this.PulseCheckBox = new System.Windows.Forms.CheckBox();
		this.SleepCheckBox = new System.Windows.Forms.CheckBox();
		this.PulseLabel = new System.Windows.Forms.Label();
		this.ZeroSecLabel = new System.Windows.Forms.Label();
		this.FiveSecLabel = new System.Windows.Forms.Label();
		this.SecondsLabel = new System.Windows.Forms.Label();
		this.BrightnessOffLabel = new System.Windows.Forms.Label();
		this.BrightnessFullLabel = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.LongClickSlider).BeginInit();
		this.LightGroupBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PulseSlider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.BrightnessSlider).BeginInit();
		base.SuspendLayout();
		this.GlobalCheckBox.AutoSize = true;
		this.GlobalCheckBox.Location = new System.Drawing.Point(72, 12);
		this.GlobalCheckBox.Name = "GlobalCheckBox";
		this.GlobalCheckBox.Size = new System.Drawing.Size(143, 17);
		this.GlobalCheckBox.TabIndex = 0;
		this.GlobalCheckBox.Text = "Use Global Settings Only";
		this.GlobalCheckBox.UseVisualStyleBackColor = true;
		this.GlobalCheckBox.CheckedChanged += new System.EventHandler(GlobalCheckBox_CheckedChanged);
		this.LongClickSlider.AutoSize = false;
		this.LongClickSlider.LargeChange = 500;
		this.LongClickSlider.Location = new System.Drawing.Point(129, 33);
		this.LongClickSlider.Maximum = 5000;
		this.LongClickSlider.Name = "LongClickSlider";
		this.LongClickSlider.Size = new System.Drawing.Size(104, 47);
		this.LongClickSlider.SmallChange = 100;
		this.LongClickSlider.TabIndex = 1;
		this.LongClickSlider.TickFrequency = 500;
		this.LongClickSlider.Scroll += new System.EventHandler(LongClickSlider_Scroll);
		this.LongClickLabel.AutoSize = true;
		this.LongClickLabel.Location = new System.Drawing.Point(35, 45);
		this.LongClickLabel.Name = "LongClickLabel";
		this.LongClickLabel.Size = new System.Drawing.Size(96, 13);
		this.LongClickLabel.TabIndex = 2;
		this.LongClickLabel.Text = "Long Click Length:";
		this.LightGroupBox.Controls.Add(this.BrightnessFullLabel);
		this.LightGroupBox.Controls.Add(this.BrightnessOffLabel);
		this.LightGroupBox.Controls.Add(this.BightnessLabel);
		this.LightGroupBox.Controls.Add(this.PulseSlider);
		this.LightGroupBox.Controls.Add(this.BrightnessSlider);
		this.LightGroupBox.Controls.Add(this.PulseCheckBox);
		this.LightGroupBox.Controls.Add(this.SleepCheckBox);
		this.LightGroupBox.Controls.Add(this.PulseLabel);
		this.LightGroupBox.Location = new System.Drawing.Point(12, 79);
		this.LightGroupBox.Name = "LightGroupBox";
		this.LightGroupBox.Size = new System.Drawing.Size(247, 128);
		this.LightGroupBox.TabIndex = 3;
		this.LightGroupBox.TabStop = false;
		this.LightGroupBox.Text = "Light";
		this.BightnessLabel.AutoSize = true;
		this.BightnessLabel.Location = new System.Drawing.Point(35, 83);
		this.BightnessLabel.Name = "BightnessLabel";
		this.BightnessLabel.Size = new System.Drawing.Size(59, 13);
		this.BightnessLabel.TabIndex = 5;
		this.BightnessLabel.Text = "Brightness:";
		this.PulseSlider.AutoSize = false;
		this.PulseSlider.LargeChange = 12;
		this.PulseSlider.Location = new System.Drawing.Point(135, 37);
		this.PulseSlider.Maximum = 24;
		this.PulseSlider.Name = "PulseSlider";
		this.PulseSlider.Size = new System.Drawing.Size(104, 37);
		this.PulseSlider.TabIndex = 3;
		this.PulseSlider.TickFrequency = 2;
		this.PulseSlider.Scroll += new System.EventHandler(PulseSlider_Scroll);
		this.BrightnessSlider.AutoSize = false;
		this.BrightnessSlider.LargeChange = 16;
		this.BrightnessSlider.Location = new System.Drawing.Point(94, 74);
		this.BrightnessSlider.Maximum = 255;
		this.BrightnessSlider.Name = "BrightnessSlider";
		this.BrightnessSlider.Size = new System.Drawing.Size(104, 45);
		this.BrightnessSlider.TabIndex = 2;
		this.BrightnessSlider.TickFrequency = 16;
		this.BrightnessSlider.Scroll += new System.EventHandler(BrightnessSlider_Scroll);
		this.PulseCheckBox.AutoSize = true;
		this.PulseCheckBox.Location = new System.Drawing.Point(18, 44);
		this.PulseCheckBox.Name = "PulseCheckBox";
		this.PulseCheckBox.Size = new System.Drawing.Size(88, 17);
		this.PulseCheckBox.TabIndex = 1;
		this.PulseCheckBox.Text = "Pulse Always";
		this.PulseCheckBox.UseVisualStyleBackColor = true;
		this.PulseCheckBox.CheckedChanged += new System.EventHandler(PulseCheckBox_CheckedChanged);
		this.SleepCheckBox.AutoSize = true;
		this.SleepCheckBox.Location = new System.Drawing.Point(18, 20);
		this.SleepCheckBox.Name = "SleepCheckBox";
		this.SleepCheckBox.Size = new System.Drawing.Size(116, 17);
		this.SleepCheckBox.TabIndex = 0;
		this.SleepCheckBox.Text = "Pulse During Sleep";
		this.SleepCheckBox.UseVisualStyleBackColor = true;
		this.SleepCheckBox.CheckedChanged += new System.EventHandler(SleepCheckBox_CheckedChanged);
		this.PulseLabel.AutoSize = true;
		this.PulseLabel.Location = new System.Drawing.Point(156, 21);
		this.PulseLabel.Name = "PulseLabel";
		this.PulseLabel.Size = new System.Drawing.Size(62, 13);
		this.PulseLabel.TabIndex = 4;
		this.PulseLabel.Text = "Pulse Rate:";
		this.ZeroSecLabel.AutoSize = true;
		this.ZeroSecLabel.Location = new System.Drawing.Point(136, 63);
		this.ZeroSecLabel.Name = "ZeroSecLabel";
		this.ZeroSecLabel.Size = new System.Drawing.Size(13, 13);
		this.ZeroSecLabel.TabIndex = 4;
		this.ZeroSecLabel.Text = "0";
		this.FiveSecLabel.AutoSize = true;
		this.FiveSecLabel.Location = new System.Drawing.Point(213, 63);
		this.FiveSecLabel.Name = "FiveSecLabel";
		this.FiveSecLabel.Size = new System.Drawing.Size(13, 13);
		this.FiveSecLabel.TabIndex = 5;
		this.FiveSecLabel.Text = "5";
		this.SecondsLabel.AutoSize = true;
		this.SecondsLabel.Location = new System.Drawing.Point(157, 63);
		this.SecondsLabel.Name = "SecondsLabel";
		this.SecondsLabel.Size = new System.Drawing.Size(47, 13);
		this.SecondsLabel.TabIndex = 6;
		this.SecondsLabel.Text = "seconds";
		this.BrightnessOffLabel.AutoSize = true;
		this.BrightnessOffLabel.Location = new System.Drawing.Point(95, 104);
		this.BrightnessOffLabel.Name = "BrightnessOffLabel";
		this.BrightnessOffLabel.Size = new System.Drawing.Size(21, 13);
		this.BrightnessOffLabel.TabIndex = 6;
		this.BrightnessOffLabel.Text = "Off";
		this.BrightnessFullLabel.AutoSize = true;
		this.BrightnessFullLabel.Location = new System.Drawing.Point(175, 104);
		this.BrightnessFullLabel.Name = "BrightnessFullLabel";
		this.BrightnessFullLabel.Size = new System.Drawing.Size(23, 13);
		this.BrightnessFullLabel.TabIndex = 7;
		this.BrightnessFullLabel.Text = "Full";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(274, 219);
		base.Controls.Add(this.SecondsLabel);
		base.Controls.Add(this.FiveSecLabel);
		base.Controls.Add(this.ZeroSecLabel);
		base.Controls.Add(this.LightGroupBox);
		base.Controls.Add(this.LongClickSlider);
		base.Controls.Add(this.GlobalCheckBox);
		base.Controls.Add(this.LongClickLabel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.MaximizeBox = false;
		base.Name = "ConfigurePowerMate";
		this.Text = "Configure PowerMate";
		((System.ComponentModel.ISupportInitialize)this.LongClickSlider).EndInit();
		this.LightGroupBox.ResumeLayout(false);
		this.LightGroupBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.PulseSlider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.BrightnessSlider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public ConfigurePowerMate()
	{
		InitializeComponent();
	}

	public void Show(DeviceNode device)
	{
		InitializeSettings(device);
		if (!base.Visible)
		{
			Show();
		}
		else
		{
			BringToFront();
		}
	}

	public void Show(DeviceNode device, IWin32Window owner)
	{
		InitializeSettings(device);
		if (!base.Visible)
		{
			Show(owner);
		}
		else
		{
			BringToFront();
		}
	}

	private void InitializeSettings(DeviceNode device)
	{
		Device = device;
		if (device.HasDevice)
		{
			base.Icon = Resources.PowerMate;
		}
		else
		{
			base.Icon = Resources.dimPowerMate;
		}
		Text = "Configure " + device.Name;
		device.SetDeviceLED();
		GlobalCheckBox.Checked = device.GlobalOnly;
		LongClick = device.HoldTime;
		SleepCheckBox.Checked = device.PulseDuringSleep;
		PulseCheckBox.Checked = device.Pulse;
		PulseRate = device.PulseRate;
		Brightness = device.LEDBrightness;
	}

	private void GlobalCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		Device.GlobalOnly = GlobalCheckBox.Checked;
	}

	private void LongClickSlider_Scroll(object sender, EventArgs e)
	{
		Device.HoldTime = LongClick;
	}

	private void SleepCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		Device.PulseDuringSleep = SleepCheckBox.Checked;
	}

	private void PulseCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		Device.Pulse = PulseCheckBox.Checked;
		BrightnessSlider.Enabled = !PulseCheckBox.Checked;
		if (BrightnessSlider.Enabled)
		{
			Device.LEDBrightness = Brightness;
		}
	}

	private void PulseSlider_Scroll(object sender, EventArgs e)
	{
		Device.PulseRate = PulseRate;
	}

	private void BrightnessSlider_Scroll(object sender, EventArgs e)
	{
		Device.LEDBrightness = Brightness;
	}
}
