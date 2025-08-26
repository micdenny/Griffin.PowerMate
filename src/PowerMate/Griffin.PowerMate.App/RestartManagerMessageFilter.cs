using System;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class RestartManagerMessageFilter : IMessageFilter
{
	private const int WM_CLOSE = 16;

	private const int WM_QUERYENDSESSION = 17;

	private const int WM_ENDSESSION = 22;

	public event EventHandler<MessageEventArgs> QueryEndSession;

	public event EventHandler<MessageEventArgs> EndSession;

	public bool PreFilterMessage(ref Message message)
	{
		if (message.Msg == 17 || message.Msg == 22)
		{
			MessageEventArgs e = new MessageEventArgs(ref message);
			switch (message.Msg)
			{
			case 17:
				OnQueryEndSession(e);
				break;
			case 22:
				OnEndSession(e);
				break;
			}
			message.Result = e.Result;
		}
		return false;
	}

	protected virtual void OnQueryEndSession(MessageEventArgs e)
	{
		if (this.QueryEndSession != null)
		{
			this.QueryEndSession(this, e);
		}
	}

	protected virtual void OnEndSession(MessageEventArgs e)
	{
		if (this.EndSession != null)
		{
			this.EndSession(this, e);
		}
	}
}
