using System;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class MessageEventArgs : EventArgs
{
	private Message _Message;

	public Message Message => _Message;

	public IntPtr Result
	{
		get
		{
			return _Message.Result;
		}
		set
		{
			_Message.Result = value;
		}
	}

	public MessageEventArgs(ref Message message)
	{
		_Message = message;
	}
}
