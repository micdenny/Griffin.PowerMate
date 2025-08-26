using System.Collections.Generic;
using System.Windows.Forms;

namespace Griffin.Input;

public class KeyboardEvent
{
	private Keys _Key;

	private KeyEventType _Type;

	public Keys Key
	{
		get
		{
			return _Key;
		}
		set
		{
			_Key = value;
		}
	}

	public KeyEventType Type
	{
		get
		{
			return _Type;
		}
		set
		{
			_Type = value;
		}
	}

	public ushort KeyCode => (ushort)_Key;

	public Keys Modifiers => _Key & Keys.Modifiers;

	public bool Alt
	{
		get
		{
			return (_Key & Keys.Alt) == Keys.Alt;
		}
		set
		{
			if (value)
			{
				_Key &= Keys.Alt;
			}
			else
			{
				_Key &= ~Keys.Alt;
			}
		}
	}

	public bool Control
	{
		get
		{
			return (_Key & Keys.Control) == Keys.Control;
		}
		set
		{
			if (value)
			{
				_Key &= Keys.Control;
			}
			else
			{
				_Key &= ~Keys.Control;
			}
		}
	}

	public bool Shift
	{
		get
		{
			return (_Key & Keys.Shift) == Keys.Shift;
		}
		set
		{
			if (value)
			{
				_Key &= Keys.Shift;
			}
			else
			{
				_Key &= ~Keys.Shift;
			}
		}
	}

	internal List<KEYBDINPUT> KeybdInput
	{
		get
		{
			List<KEYBDINPUT> list = new List<KEYBDINPUT>();
			if (_Type == KeyEventType.KeyUp)
			{
				list.Add(new KEYBDINPUT
				{
					wVk = (ushort)(_Key & Keys.KeyCode),
					dwFlags = _Type
				});
			}
			if (Alt)
			{
				list.Add(new KEYBDINPUT
				{
					wVk = 18,
					dwFlags = _Type
				});
			}
			if (Control)
			{
				list.Add(new KEYBDINPUT
				{
					wVk = 17,
					dwFlags = _Type
				});
			}
			if (Shift)
			{
				list.Add(new KEYBDINPUT
				{
					wVk = 16,
					dwFlags = _Type
				});
			}
			if (_Type == KeyEventType.KeyDown)
			{
				list.Add(new KEYBDINPUT
				{
					wVk = (ushort)(_Key & Keys.KeyCode),
					dwFlags = _Type
				});
			}
			return list;
		}
	}

	public KeyboardEvent(Keys key, KeyEventType type)
	{
		_Key = key;
		_Type = type;
	}
}
