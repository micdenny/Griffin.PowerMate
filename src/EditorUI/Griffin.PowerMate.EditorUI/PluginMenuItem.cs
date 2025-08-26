using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class PluginMenuItem : ToolStripMenuItem
{
	private IPMActionPlugin Plugin;

	private Image PluginImage;

	private bool _Shown = true;

	public override string Text
	{
		get
		{
			if (Plugin != null)
			{
				return Plugin.Name;
			}
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	public override Image Image => PluginImage;

	public bool Shown
	{
		get
		{
			return _Shown;
		}
		set
		{
			if (_Shown != value)
			{
				_Shown = value;
				Invalidate();
			}
		}
	}

	public new bool Visible
	{
		get
		{
			return base.Visible & _Shown;
		}
		set
		{
			base.Visible = value;
		}
	}

	public PluginMenuItem(IPMActionPlugin plugin)
	{
		Plugin = plugin;
		if (plugin != null)
		{
			if (plugin.Icon != null)
			{
				PluginImage = plugin.Icon.ToBitmap();
			}
			IComputerAction[] availableActions = plugin.AvailableActions;
			foreach (IComputerAction caction in availableActions)
			{
				base.DropDownItems.Add(new CActionMenuItem(caction));
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && PluginImage != null)
		{
			PluginImage.Dispose();
		}
		base.Dispose(disposing);
	}
}
