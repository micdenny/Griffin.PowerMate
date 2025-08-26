using System.Collections.Generic;
using System.Windows.Forms;

namespace Griffin.Input;

public static class Keyboard
{
	public static bool SendKeyboardEvent(Keys key, KeyEventType type)
	{
		if (InputBase.SendInput(new KeyboardEvent(key, type).KeybdInput.ToArray()) != 0)
		{
			return true;
		}
		return false;
	}

	public static uint SendKeyboardEvent(params KeyboardEvent[] keyboardEvents)
	{
		List<KEYBDINPUT> list = new List<KEYBDINPUT>();
		for (int i = 0; i < keyboardEvents.Length; i++)
		{
			list.AddRange(keyboardEvents[i].KeybdInput);
		}
		return InputBase.SendInput(list.ToArray());
	}

	public static bool KeyDown(Keys key)
	{
		return SendKeyboardEvent(key, KeyEventType.KeyDown);
	}

	public static bool KeyUp(Keys key)
	{
		return SendKeyboardEvent(key, KeyEventType.KeyUp);
	}

	public static bool KeyPress(Keys key)
	{
		List<KEYBDINPUT> list = new List<KEYBDINPUT>(new KeyboardEvent(key, KeyEventType.KeyDown).KeybdInput);
		list.AddRange(new KeyboardEvent(key, KeyEventType.KeyUp).KeybdInput);
		if (InputBase.SendInput(list.ToArray()) >= 2)
		{
			return true;
		}
		return false;
	}
}
