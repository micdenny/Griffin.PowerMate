using System.Runtime.InteropServices;

namespace Griffin.Input;

[StructLayout(LayoutKind.Explicit)]
internal struct INPUT_64
{
	[FieldOffset(0)]
	public InputType type;

	[FieldOffset(8)]
	public MOUSEINPUT mi;

	[FieldOffset(8)]
	public KEYBDINPUT ki;

	[FieldOffset(8)]
	public HARDWAREINPUT hi;
}
