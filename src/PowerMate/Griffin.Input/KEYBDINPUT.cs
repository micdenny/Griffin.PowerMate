using System;

namespace Griffin.Input;

internal struct KEYBDINPUT
{
	public ushort wVk;

	public ushort wScan;

	public KeyEventType dwFlags;

	public uint time;

	public IntPtr dwExtraInfo;
}
