using System.Windows.Forms;

namespace Griffin.PowerMate.SendKeysActions;

internal class SendKeysEventArgs : KeyEventArgs
{
	private bool _Win;

	public bool Win => _Win;

	public SendKeysEventArgs(Keys keyData, bool win)
		: base(keyData)
	{
		_Win |= win;
	}

	public SendKeysEventArgs(Keys keyData, ModifierKeysState modifierState)
		: base(keyData | modifierState.ModifierKeys)
	{
		_Win |= modifierState.Win;
	}
}
