using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Timers;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class PowerMatesHeaderColumn : HeaderColumn
{
	private Brush NoPMSurroundBrush = SystemBrushes.Control;

	private Brush NoPMTextBrush = SystemBrushes.WindowText;

	private string NoPMMessage = "No PowerMates";

	private int NoPMMessagePadding = 5;

	private double PulseTimerInterval = 1000.0;

	private System.Timers.Timer PulseTimer = new System.Timers.Timer();

	private DeviceColumnItem Clicked;

	private PowerMateDoc myPmDoc;

	private EventHandler<NodeContainerEventArgs<DeviceNode>> DeviceAdded;

	private EventHandler<NodeContainerEventArgs<DeviceNode>> DeviceRemoved;

	private EventHandler<NodeContainerEventArgs<DeviceNode>> DeviceReplaced;

	private OpenFileDialog ImportPowerMates;

	private SaveFileDialog ExportPowerMates;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PowerMateDoc PmDoc
	{
		set
		{
			if (myPmDoc != null)
			{
				myPmDoc.ChildNodeInserted -= DeviceAdded;
				myPmDoc.ChildNodeRemoved -= DeviceRemoved;
				myPmDoc.ChildNodeReplaced -= DeviceReplaced;
				foreach (DeviceColumnItem item in base.Items)
				{
					item.Dispose();
				}
			}
			myPmDoc = value;
			base.Items.Clear();
			if (value != null)
			{
				value.ChildNodeInserted += DeviceAdded;
				value.ChildNodeRemoved += DeviceRemoved;
				value.ChildNodeReplaced += DeviceReplaced;
				foreach (DeviceNode item2 in value)
				{
					if (item2.Name == null)
					{
						item2.Name = NextDeviceNodeName;
					}
					base.Items.Add(new DeviceColumnItem(item2));
				}
			}
			if (base.Items.Count > 0)
			{
				SelectOne(0);
			}
		}
	}

	protected string NextDeviceNodeName
	{
		get
		{
			int num = 1;
			bool flag = false;
			while (!flag)
			{
				flag = true;
				foreach (DeviceNode item in myPmDoc)
				{
					if (item.Name != null)
					{
						string text = item.Name.Trim().ToLower();
						if (text == "powermate " + num || text == "powermate" + num)
						{
							num++;
							flag = false;
							break;
						}
					}
				}
			}
			return "PowerMate " + num;
		}
	}

	public PowerMatesHeaderColumn()
	{
		InitializeFileDialogs();
		DeviceAdded = PmDoc_DeviceAdded;
		DeviceRemoved = PmDoc_DeviceRemoved;
		DeviceReplaced = PmDoc_DeviceReplaced;
		PulseTimer.Elapsed += PulseTimer_Elapsed;
		PulseTimer.AutoReset = false;
	}

	private void InitializeFileDialogs()
	{
		ImportPowerMates = new OpenFileDialog();
		ExportPowerMates = new SaveFileDialog();
		ImportPowerMates.DefaultExt = "pmsettings";
		ImportPowerMates.Filter = "PowerMate Settings|*.pmsettings|All files|*.*";
		ImportPowerMates.SupportMultiDottedExtensions = true;
		ImportPowerMates.Title = "Import PowerMate Settings";
		ExportPowerMates.DefaultExt = "pmsettings";
		ExportPowerMates.Filter = "PowerMate Settings|*.pmsettings|All files|*.*";
		ExportPowerMates.OverwritePrompt = false;
		ExportPowerMates.SupportMultiDottedExtensions = true;
		ExportPowerMates.Title = "Export PowerMate Settings";
	}

	public DeviceNode AddPowerMate()
	{
		DeviceNode deviceNode = new DeviceNode();
		deviceNode.Name = NextDeviceNodeName;
		myPmDoc.Add(deviceNode);
		if (PowerMateApp.PowerMates.Length >= myPmDoc.Count)
		{
			myPmDoc.AssignPowerMates(PowerMateApp.PowerMates, DeviceAssignment.Default);
		}
		SelectOne(base.Items.Count - 1);
		return deviceNode;
	}

	public int RemoveSelected()
	{
		bool flag = false;
		int currentIndex = base.CurrentIndex;
		int count = base.SelectedItems.Count;
		foreach (DeviceColumnItem selectedItem in base.SelectedItems)
		{
			if (selectedItem.Node.HasDevice)
			{
				flag = true;
			}
			selectedItem.Node.Active = false;
			myPmDoc.Remove(selectedItem.Node);
		}
		if (flag)
		{
			myPmDoc.AssignPowerMates(PowerMateApp.PowerMates, DeviceAssignment.Default);
		}
		if (currentIndex >= 0 && currentIndex < base.Items.Count)
		{
			SelectOne(currentIndex);
		}
		else if (base.Items.Count > 0)
		{
			SelectOne(base.Items.Count - 1);
		}
		return count;
	}

	public void SwitchSettings(int index1, int index2)
	{
		DeviceNode deviceNode = myPmDoc[index1];
		string lastPath = deviceNode.LastPath;
		myPmDoc[index1].SetSetting("lastpath", myPmDoc[index2].LastPath);
		myPmDoc[index2].SetSetting("lastpath", lastPath);
		myPmDoc[index1] = myPmDoc[index2];
		myPmDoc[index2] = deviceNode;
		PowerMateApp.PowerMateDoc.AssignPowerMates(PowerMateApp.PowerMates, DeviceAssignment.Default);
	}

	public void ReplaceSelected(DeviceColumnItem item)
	{
		for (int i = 0; i < base.Items.Count; i++)
		{
			if (base.Items[i].Selected && base.Items[i] != item)
			{
				string name = myPmDoc[i].Name;
				string lastPath = myPmDoc[i].LastPath;
				myPmDoc[i] = (DeviceNode)item.Node.Clone();
				myPmDoc[i].Name = name;
				myPmDoc[i].SetSetting("lastpath", lastPath);
			}
		}
		PowerMateApp.PowerMateDoc.AssignPowerMates(PowerMateApp.PowerMates, DeviceAssignment.Default);
	}

	public void DuplicateSelected()
	{
		foreach (DeviceColumnItem selectedItem in base.SelectedItems)
		{
			DeviceNode deviceNode = (DeviceNode)selectedItem.Node.Clone();
			deviceNode.Name += " copy";
			myPmDoc.Add(deviceNode);
		}
	}

	public int ImportSettings()
	{
		if (ImportPowerMates.ShowDialog() == DialogResult.OK)
		{
			int num = 0;
			int currentIndex = base.CurrentIndex;
			try
			{
				PowerMateDoc powerMateDoc = new PowerMateDoc(ImportPowerMates.FileName, PowerMateApp.ActionPlugins);
				if (currentIndex >= 0 && base.Items[currentIndex].Selected && powerMateDoc.Count > 0)
				{
					DeviceNode deviceNode = powerMateDoc[0];
					string lastPath = deviceNode.LastPath;
					powerMateDoc.RemoveAt(0);
					deviceNode.SetSetting("lastpath", myPmDoc[currentIndex].LastPath);
					myPmDoc[currentIndex].SetSetting("lastpath", lastPath);
					powerMateDoc.Add(myPmDoc[currentIndex]);
					myPmDoc[currentIndex] = deviceNode;
				}
				foreach (DeviceNode item in powerMateDoc)
				{
					myPmDoc.Add(item);
					num++;
				}
			}
			catch
			{
				MessageBox.Show("The selected file does not seem to be a valid settings file.", "Error Importing File", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			if (num > 0)
			{
				PowerMateApp.PowerMateDoc.AssignPowerMates(PowerMateApp.PowerMates, DeviceAssignment.Default);
				if (currentIndex >= 0)
				{
					SelectAdd(currentIndex);
				}
				else
				{
					SelectAdd(base.Items.Count - 1);
				}
			}
			return num;
		}
		return -1;
	}

	public bool ExportSelected()
	{
		ColumnItemCollection selectedItems = base.SelectedItems;
		if (selectedItems.Count > 0)
		{
			ExportPowerMates.FileName = selectedItems[0].Text;
		}
		if (ExportPowerMates.ShowDialog() == DialogResult.OK)
		{
			PowerMateDoc powerMateDoc = new PowerMateDoc();
			foreach (DeviceColumnItem selectedItem in base.SelectedItems)
			{
				powerMateDoc.Add(selectedItem.Node);
			}
			return powerMateDoc.Save(ExportPowerMates.FileName, overwrite: true);
		}
		return false;
	}

	private void PmDoc_DeviceAdded(object sender, NodeContainerEventArgs<DeviceNode> e)
	{
		if (e.Node.Name == null)
		{
			e.Node.Name = NextDeviceNodeName;
		}
		base.Items.Insert(e.Index, new DeviceColumnItem(e.Node));
	}

	private void PmDoc_DeviceRemoved(object sender, NodeContainerEventArgs<DeviceNode> e)
	{
		if (e.Node == ((DeviceColumnItem)base.Items[e.Index]).Node)
		{
			DeviceColumnItem deviceColumnItem = (DeviceColumnItem)base.Items[e.Index];
			base.Items.RemoveAt(e.Index);
			deviceColumnItem.Dispose();
		}
	}

	private void PmDoc_DeviceReplaced(object sender, NodeContainerEventArgs<DeviceNode> e)
	{
		DeviceColumnItem deviceColumnItem = (DeviceColumnItem)base.Items[e.Index];
		base.Items[e.Index] = new DeviceColumnItem(myPmDoc[e.Index]);
		deviceColumnItem.Dispose();
	}

	protected override void OnItemMouseClick(IColumnItem item)
	{
		if (PulseTimer.Enabled && Clicked != null)
		{
			Clicked.Node.SetDeviceLED();
		}
		Clicked = (DeviceColumnItem)item;
		if (Clicked.Node.HasDevice)
		{
			Clicked.Node.Device.PulseSpeed = 12;
			Clicked.Node.Device.Pulse = true;
			PulseTimer.Interval = PulseTimerInterval;
			PulseTimer.Start();
		}
		base.OnItemMouseClick(item);
	}

	private void PulseTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (Clicked != null)
		{
			Clicked.Node.SetDeviceLED();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		if (myPmDoc == null || myPmDoc.Count == 0)
		{
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			SizeF sizeF = graphics.MeasureString(NoPMMessage, Font);
			RectangleF rect = new RectangleF(((float)base.ClientSize.Width - sizeF.Width) / 2f - (float)NoPMMessagePadding, ((float)base.ClientSize.Height - sizeF.Height) / 2f - (float)NoPMMessagePadding, sizeF.Width + (float)(NoPMMessagePadding * 2), sizeF.Height + (float)(NoPMMessagePadding * 2));
			graphics.FillRectangle(NoPMSurroundBrush, rect);
			graphics.FillEllipse(NoPMSurroundBrush, rect.X - rect.Height / 2f, rect.Y, rect.Height, rect.Height);
			graphics.FillEllipse(NoPMSurroundBrush, rect.Right - rect.Height / 2f, rect.Y, rect.Height, rect.Height);
			graphics.DrawString(NoPMMessage, Font, NoPMTextBrush, rect.X + (float)NoPMMessagePadding, rect.Y + (float)NoPMMessagePadding);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			PulseTimer.Dispose();
			ImportPowerMates.Dispose();
			ExportPowerMates.Dispose();
			foreach (DeviceColumnItem item in base.Items)
			{
				item.Dispose();
			}
		}
		base.Dispose(disposing);
	}
}
