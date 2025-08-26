using System;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class ActivateFilter : IMessageFilter
{
	private const int WM_ACTIVATE = 6;

	private const int WA_ACTIVE = 1;

	private const int WA_CLICKACTIVE = 2;

	public event EventHandler Activated;

	public bool PreFilterMessage(ref Message m)
	{
		if (m.Msg == 6 && (int)m.WParam == 1)
		{
			OnActivated(EventArgs.Empty);
		}
		return false;
	}

	protected virtual void OnActivated(EventArgs e)
	{
		if (this.Activated != null)
		{
			this.Activated(this, e);
		}
	}
}
