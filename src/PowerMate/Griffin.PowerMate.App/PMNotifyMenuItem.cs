using System;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class PMNotifyMenuItem : ToolStripMenuItem
{
	private IPowerMateUIPlugin UIPlugin;

	public IPowerMateUIPlugin Plugin => UIPlugin;

	public override string Text
	{
		get
		{
			return UIPlugin.Name;
		}
		set
		{
			base.Text = value;
		}
	}

	public PMNotifyMenuItem(IPowerMateUIPlugin plugin)
	{
		UIPlugin = plugin;
		UIPlugin_StatusChanged(UIPlugin, EventArgs.Empty);
		UIPlugin.StatusChanged += UIPlugin_StatusChanged;
	}

	private void UIPlugin_StatusChanged(object sender, EventArgs e)
	{
		base.Checked = UIPlugin.Status == UIStatus.Open;
	}
}
