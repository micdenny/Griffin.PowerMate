using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Griffin.Input;

namespace Griffin.PowerMate.SendKeysActions;

internal struct ModifierKeysState
{
	private Keys _ModifierKeys;

	private bool _Win;

	public static readonly ModifierKeysState Empty;

	public Keys ModifierKeys
	{
		get
		{
			return _ModifierKeys;
		}
		set
		{
			_ModifierKeys = value & Keys.Modifiers;
		}
	}

	public bool Shift
	{
		get
		{
			return (ModifierKeys & Keys.Shift) == Keys.Shift;
		}
		set
		{
			_ModifierKeys |= Keys.Shift;
		}
	}

	public bool Ctrl
	{
		get
		{
			return (ModifierKeys & Keys.Control) == Keys.Control;
		}
		set
		{
			_ModifierKeys |= Keys.Control;
		}
	}

	public bool Alt
	{
		get
		{
			return (ModifierKeys & Keys.Alt) == Keys.Alt;
		}
		set
		{
			_ModifierKeys |= Keys.Alt;
		}
	}

	public bool Win
	{
		get
		{
			return _Win;
		}
		set
		{
			_Win = value;
		}
	}

	public bool IsEmpty
	{
		get
		{
			if (ModifierKeys == Keys.None)
			{
				return !Win;
			}
			return false;
		}
	}

	public static ModifierKeysState Present
	{
		get
		{
			byte[] array = new byte[256];
			bool flag = false;
			if (GetKeyboardState(array))
			{
				flag |= (array[91] & 0x80) == 128;
				flag |= (array[92] & 0x80) == 128;
			}
			return new ModifierKeysState(Control.ModifierKeys, flag);
		}
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool GetKeyboardState(byte[] lpKeyState);

	public ModifierKeysState(Keys modifierKeys, bool win)
	{
		_ModifierKeys = modifierKeys;
		_Win = win;
	}

	public ModifierKeysState(bool shift, bool ctrl, bool alt, bool win)
		: this(Keys.None, win)
	{
		Shift = shift;
		Ctrl = ctrl;
		Alt = alt;
	}

	public void ClearAll()
	{
		_ModifierKeys = Keys.None;
		Win = false;
	}

	public void Set()
	{
		List<KeyboardEvent> list = new List<KeyboardEvent>();
		list.Add(new KeyboardEvent(Keys.Modifiers & ~ModifierKeys, KeyEventType.KeyUp));
		list.Add(new KeyboardEvent(ModifierKeys, KeyEventType.KeyDown));
		byte[] array = new byte[256];
		if (GetKeyboardState(array))
		{
			_ = array[91];
			_ = array[92];
			if (!Win)
			{
				if ((array[91] & 0x80) == 128)
				{
					list.Add(new KeyboardEvent(Keys.LWin, KeyEventType.KeyUp));
				}
				if ((array[92] & 0x80) == 128)
				{
					list.Add(new KeyboardEvent(Keys.RWin, KeyEventType.KeyUp));
				}
			}
		}
		Keyboard.SendKeyboardEvent(list.ToArray());
	}

	public bool ChangeKey(Keys modifierKey, bool down)
	{
		bool flag = false;
		Keys keys = modifierKey & Keys.Modifiers;
		modifierKey &= Keys.KeyCode;
		switch (modifierKey)
		{
		case Keys.ShiftKey:
		case Keys.LShiftKey:
		case Keys.RShiftKey:
			keys |= Keys.Shift;
			break;
		case Keys.ControlKey:
		case Keys.LControlKey:
		case Keys.RControlKey:
			keys |= Keys.Control;
			break;
		case Keys.Menu:
		case Keys.LMenu:
		case Keys.RMenu:
			keys |= Keys.Alt;
			break;
		case Keys.LWin:
		case Keys.RWin:
			flag |= Win != down;
			Win = down;
			break;
		default:
			return false;
		}
		keys = ((!down) ? (ModifierKeys & ~keys) : (keys | ModifierKeys));
		flag |= ModifierKeys != keys;
		_ModifierKeys = keys;
		return flag;
	}

	public static string GetModifierKeyString(Keys modifierKey)
	{
		string result = null;
		switch (modifierKey)
		{
		case Keys.ShiftKey:
		case Keys.LShiftKey:
		case Keys.RShiftKey:
		case Keys.Shift:
			result = "Shift";
			break;
		case Keys.ControlKey:
		case Keys.LControlKey:
		case Keys.RControlKey:
		case Keys.Control:
			result = "Ctrl";
			break;
		case Keys.Menu:
		case Keys.LMenu:
		case Keys.RMenu:
		case Keys.Alt:
			result = "Alt";
			break;
		case Keys.LWin:
		case Keys.RWin:
			result = "Win";
			break;
		}
		return result;
	}

	public static bool IsModifier(Keys key)
	{
		bool result = false;
		switch (key)
		{
		case Keys.ShiftKey:
		case Keys.ControlKey:
		case Keys.Menu:
		case Keys.LWin:
		case Keys.RWin:
		case Keys.LShiftKey:
		case Keys.RShiftKey:
		case Keys.LControlKey:
		case Keys.RControlKey:
		case Keys.LMenu:
		case Keys.RMenu:
		case Keys.Shift:
		case Keys.Control:
		case Keys.Alt:
			result = true;
			break;
		}
		return result;
	}

	public override string ToString()
	{
		string text = (Shift ? GetModifierKeyString(Keys.Shift) : "");
		if (Ctrl)
		{
			text = AppendPlusToKeyStringIfNeeded(text) + GetModifierKeyString(Keys.Control);
		}
		if (Alt)
		{
			text = AppendPlusToKeyStringIfNeeded(text) + GetModifierKeyString(Keys.Alt);
		}
		if (Win)
		{
			text = AppendPlusToKeyStringIfNeeded(text) + GetModifierKeyString(Keys.LWin);
		}
		return text;
	}

	private string AppendPlusToKeyStringIfNeeded(string keyString)
	{
		if (!string.IsNullOrEmpty(keyString))
		{
			return keyString + "+";
		}
		return keyString;
	}
}
