using System;

namespace Griffin.Input;

internal struct MOUSEINPUT
{
	public int dx;

	public int dy;

	public int mouseData;

	public MouseEventType dwFlags;

	public uint time;

	public IntPtr dwExtraInfo;
}
