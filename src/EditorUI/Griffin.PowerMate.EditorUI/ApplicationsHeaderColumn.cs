using System;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class ApplicationsHeaderColumn : HeaderColumn
{
	private ShortcutTargetReplacements ReplaceImageNameDict = new ShortcutTargetReplacements();

	private AddApplicationDialog AddAppDialog = new AddApplicationDialog();

	private DeviceNode myDevice;

	private EventHandler<NodeContainerEventArgs<AppNode>> ApplicationAdded;

	private EventHandler<NodeContainerEventArgs<AppNode>> ApplicationRemoved;

	private EventHandler<NodeContainerEventArgs<AppNode>> ApplicationReplaced;

	public DeviceNode Device
	{
		set
		{
			if (myDevice != null)
			{
				myDevice.ChildNodeInserted -= ApplicationAdded;
				myDevice.ChildNodeRemoved -= ApplicationRemoved;
				myDevice.ChildNodeReplaced -= ApplicationReplaced;
				foreach (ApplicationColumnItem item in base.Items)
				{
					item.Dispose();
				}
			}
			myDevice = value;
			base.Items.Clear();
			if (value != null)
			{
				value.ChildNodeInserted += ApplicationAdded;
				value.ChildNodeRemoved += ApplicationRemoved;
				value.ChildNodeReplaced += ApplicationReplaced;
				foreach (AppNode item2 in value)
				{
					base.Items.Add(new ApplicationColumnItem(item2));
				}
			}
			if (base.Items.Count > 0)
			{
				SelectOne(0);
			}
		}
	}

	public ApplicationsHeaderColumn()
	{
		ApplicationAdded = Device_ApplicationAdded;
		ApplicationRemoved = Device_ApplicationRemoved;
		ApplicationReplaced = Device_ApplicationReplaced;
	}

	public AppNode AddApplication()
	{
		AppNode result = null;
		if (myDevice != null)
		{
			bool flag = myDevice.Contains("");
			if (AddAppDialog.ShowDialog(!flag) == DialogResult.OK)
			{
				result = AddApplication(AddAppDialog.ImageName, AddAppDialog.SettingName, AddAppDialog.Path, -1);
			}
		}
		return result;
	}

	public AppNode AddApplication(string image, string name, string iconPath, int position)
	{
		AppNode appNode = new AppNode(image);
		if (name != null && name != image)
		{
			appNode.Name = name;
		}
		if (!string.IsNullOrEmpty(iconPath))
		{
			appNode.SetSetting("iconPath", iconPath);
		}
		if (AddApplication(appNode, position))
		{
			return appNode;
		}
		return null;
	}

	public bool AddApplication(AppNode appNode, int position)
	{
		if (myDevice != null)
		{
			if (!myDevice.Contains(appNode.Image))
			{
				if (position >= 0 && position < myDevice.Count)
				{
					myDevice.Insert(position, appNode);
					SelectOne(position);
				}
				else
				{
					myDevice.Add(appNode);
					SelectOne(base.Items.Count - 1);
				}
				return true;
			}
			ShowSettingExistsMessage(appNode.Image);
		}
		return false;
	}

	public DialogResult ShowSettingExistsMessage(string appName)
	{
		if (myDevice != null)
		{
			return ShowSettingExistsMessage(myDevice.Name, appName);
		}
		return DialogResult.Abort;
	}

	public DialogResult ShowSettingExistsMessage(string pmName, string appName)
	{
		return MessageBox.Show("\"" + pmName + "\" already has a setting for the Application \"" + appName + "\"." + Environment.NewLine + "You may want to review and modify the existing settings for this application.", "Setting Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	public int RemoveSelectedApplications()
	{
		int count = base.SelectedItems.Count;
		foreach (ApplicationColumnItem selectedItem in base.SelectedItems)
		{
			myDevice.Remove(selectedItem.Node);
		}
		return count;
	}

	private void Device_ApplicationAdded(object sender, NodeContainerEventArgs<AppNode> e)
	{
		base.Items.Insert(e.Index, new ApplicationColumnItem(e.Node));
	}

	private void Device_ApplicationRemoved(object sender, NodeContainerEventArgs<AppNode> e)
	{
		if (e.Node == ((ApplicationColumnItem)base.Items[e.Index]).Node)
		{
			ApplicationColumnItem applicationColumnItem = (ApplicationColumnItem)base.Items[e.Index];
			base.Items.RemoveAt(e.Index);
			applicationColumnItem.Dispose();
		}
	}

	private void Device_ApplicationReplaced(object sender, NodeContainerEventArgs<AppNode> e)
	{
		ApplicationColumnItem applicationColumnItem = (ApplicationColumnItem)base.Items[e.Index];
		base.Items[e.Index] = new ApplicationColumnItem(myDevice[e.Index]);
		applicationColumnItem.Dispose();
	}

	protected override void OnDragEnter(DragEventArgs drgevent)
	{
		DragDropEffects effect = DragDropEffects.None;
		if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
		{
			string[] array = (string[])drgevent.Data.GetData(DataFormats.FileDrop, autoConvert: false);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.ToLower().EndsWith(".exe"))
				{
					effect = DragDropEffects.All;
					break;
				}
				if (text.ToLower().EndsWith(".lnk"))
				{
					WshShellClass wshShellClass = new WshShellClass();
					IWshShortcut wshShortcut = wshShellClass.CreateShortcut(text);
					if (wshShortcut.TargetPath.ToLower().EndsWith(".exe"))
					{
						effect = DragDropEffects.All;
						break;
					}
					wshShellClass.Dispose();
					wshShortcut.Dispose();
				}
			}
		}
		else if (drgevent.Data.GetDataPresent(typeof(ApplicationColumnItem)))
		{
			effect = DragDropEffects.All;
		}
		drgevent.Effect = effect;
		base.OnDragEnter(drgevent);
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		string[] array = (string[])drgevent.Data.GetData(DataFormats.FileDrop, autoConvert: false);
		string[] array2 = array;
		foreach (string text in array2)
		{
			string text2 = null;
			string text3 = null;
			int num = text.LastIndexOf('\\') + 1;
			int length = text.LastIndexOf('.') - num;
			string name = text.Substring(num, length);
			if (text.ToLower().EndsWith(".lnk"))
			{
				WshShellClass wshShellClass = new WshShellClass();
				IWshShortcut wshShortcut = wshShellClass.CreateShortcut(text);
				text2 = wshShortcut.TargetPath;
				text3 = wshShortcut.IconLocation;
				if (text3.Length < 3)
				{
					text3 = text2;
				}
				wshShellClass.Dispose();
				wshShortcut.Dispose();
			}
			else
			{
				text2 = (text3 = text);
			}
			if (text2.ToLower().EndsWith(".exe"))
			{
				num = text2.LastIndexOf('\\') + 1;
				length = text2.LastIndexOf('.') - num;
				text2 = text2.Substring(num, length);
				string value = null;
				if (text.ToLower().EndsWith(".lnk") && ReplaceImageNameDict.TryGetValue(text2, out value))
				{
					text2 = value;
				}
				int position = HitTestIndex(PointToClient(new Point(drgevent.X, drgevent.Y)));
				AddApplication(text2, name, text3, position);
			}
		}
		base.OnDragDrop(drgevent);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (ApplicationColumnItem item in base.Items)
			{
				item.Dispose();
			}
			AddAppDialog.Dispose();
		}
		base.Dispose(disposing);
	}
}
