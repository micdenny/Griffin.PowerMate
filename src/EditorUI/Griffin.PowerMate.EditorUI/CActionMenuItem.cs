using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class CActionMenuItem : ToolStripMenuItem
{
	private IComputerAction CAction;

	private Image CActionImage;

	private bool _Shown = true;

	public IComputerAction ComputerAction => CAction;

	public override string Text
	{
		get
		{
			if (CAction != null)
			{
				return CAction.Name;
			}
			return "No Action";
		}
	}

	public override Image Image => CActionImage;

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

	public CActionMenuItem(IComputerAction caction)
	{
		CAction = caction;
		if (caction != null && caction.Icon != null)
		{
			CActionImage = caction.Icon.ToBitmap();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (CActionImage != null)
		{
			CActionImage.Dispose();
		}
		base.Dispose(disposing);
	}
}
