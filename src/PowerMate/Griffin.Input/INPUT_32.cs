using System.Runtime.InteropServices;

namespace Griffin.Input;

[StructLayout(LayoutKind.Explicit)]
internal struct INPUT_32
{
	[FieldOffset(0)]
	public InputType type;

	[FieldOffset(4)]
	public MOUSEINPUT mi;

	[FieldOffset(4)]
	public KEYBDINPUT ki;

	[FieldOffset(4)]
	public HARDWAREINPUT hi;
}
