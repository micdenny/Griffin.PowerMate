using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Griffin.PowerMate.SendKeysActions;

internal class SendKeysTextBox : TextBox
{
	private struct KBDLLHOOKSTRUCT
	{
		public uint vkCode;

		public uint scanCode;

		public uint flags;

		public uint time;

		public IntPtr dwExtraInfo;
	}

	private delegate int KeyboardHookProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

	private const int WH_KEYBOARD = 2;

	private const int WH_KEYBOARD_LL = 13;

	private const int WM_KEYDOWN = 256;

	private const int WM_KEYUP = 257;

	private const int WM_SYSKEYDOWN = 260;

	private const int WM_SYSKEYUP = 261;

	private KeyboardHookProc KeyboardHookCallbackHandler;

	private IntPtr KeyboardHookHandle;

	private ModifierKeysState ModifierKeysState = default(ModifierKeysState);

	private bool StartNewKey = true;

	private KeysConverter KeyConverter = new KeysConverter();

	public event EventHandler<SendKeysEventArgs> KeyAdded;

	public event EventHandler KeysCleared;

	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr SetWindowsHookEx(int idHoook, KeyboardHookProc lpfn, IntPtr hInstance, int threadId);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool UnhookWindowsHookEx(IntPtr hhk);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern int CallNextHookEx(IntPtr hhk, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

	public SendKeysTextBox()
	{
		KeyboardHookCallbackHandler = KeyboardHookCallback;
		KeyboardHookHandle = SetWindowsHookEx(13, KeyboardHookCallbackHandler, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
	}

	public Keys StringToKey(string key)
	{
		return (Keys)KeyConverter.ConvertFromString(key);
	}

	public bool AddKey(Keys key)
	{
		if (!StartNewKey)
		{
			OnKeyAdded(new SendKeysEventArgs(Keys.None, ModifierKeysState));
		}
		AddKeyText(key, ModifierKeysState.Empty, startNewKey: true);
		OnKeyAdded(new SendKeysEventArgs(key, ModifierKeysState.Empty));
		return true;
	}

	public bool AddKey(string key)
	{
		try
		{
			return AddKey(StringToKey(key));
		}
		catch
		{
			return false;
		}
	}

	public void ClearKeys()
	{
		Text = "";
		OnKeysCleared(EventArgs.Empty);
	}

	public override bool PreProcessMessage(ref Message msg)
	{
		bool flag = false;
		if (msg.Msg == 256 || msg.Msg == 257 || msg.Msg == 260 || msg.Msg == 261)
		{
			flag = ProcessKey(msg.Msg, (Keys)msg.WParam.ToInt32());
		}
		if (!flag)
		{
			flag = base.PreProcessMessage(ref msg);
		}
		return flag;
	}

	protected override void OnEnter(EventArgs e)
	{
		base.OnEnter(e);
		ClearKeys();
		ModifierKeysState = ModifierKeysState.Present;
	}

	protected override void OnLeave(EventArgs e)
	{
		if (!StartNewKey)
		{
			OnKeyAdded(new SendKeysEventArgs(Keys.None, ModifierKeysState));
		}
		ModifierKeysState.Set();
		base.OnLeave(e);
	}

	protected virtual void OnKeyAdded(SendKeysEventArgs e)
	{
		StartNewKey = true;
		if (this.KeyAdded != null)
		{
			this.KeyAdded(this, e);
		}
	}

	protected virtual void OnKeysCleared(EventArgs e)
	{
		if (this.KeysCleared != null)
		{
			this.KeysCleared(this, e);
		}
	}

	protected override void Dispose(bool disposing)
	{
		UnhookWindowsHookEx(KeyboardHookHandle);
		base.Dispose(disposing);
	}

	protected bool ProcessKey(int msg, Keys keyData)
	{
		bool flag = msg == 256 || msg == 260;
		ModifierKeysState modifierState = (StartNewKey ? ModifierKeysState : ModifierKeysState.Empty);
		if (ModifierKeysState.IsModifier(keyData))
		{
			if (!flag && !StartNewKey)
			{
				OnKeyAdded(new SendKeysEventArgs(Keys.None, ModifierKeysState));
			}
			if (ModifierKeysState.ChangeKey(keyData, flag) && flag)
			{
				AddKeyText(keyData, modifierState, StartNewKey);
				StartNewKey = false;
			}
		}
		else if (flag)
		{
			AddKeyText(keyData, modifierState, StartNewKey);
			OnKeyAdded(new SendKeysEventArgs(Keys.None, ModifierKeysState));
		}
		return true;
	}

	private void AddKeyText(Keys key, ModifierKeysState modifierState, bool startNewKey)
	{
		modifierState.ChangeKey(key, down: true);
		key &= Keys.KeyCode;
		string text = modifierState.ToString();
		if (!ModifierKeysState.IsModifier(key) && key != Keys.None)
		{
			if (!modifierState.IsEmpty)
			{
				text += "+";
			}
			text += KeyConverter.ConvertToString(key);
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (!startNewKey)
			{
				Text += "+";
			}
			else if (Text != "")
			{
				Text += ", ";
			}
			Text += text;
		}
		base.SelectionStart = Text.Length;
	}

	private int KeyboardHookCallback(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
	{
		if (Focused && nCode >= 0 && ProcessKey(wParam, (Keys)lParam.vkCode))
		{
			return -1;
		}
		return CallNextHookEx(KeyboardHookHandle, nCode, wParam, ref lParam);
	}
}
