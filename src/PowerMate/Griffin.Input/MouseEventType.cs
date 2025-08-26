namespace Griffin.Input;

public enum MouseEventType : uint
{
	Move = 1u,
	LeftDown = 2u,
	LeftUp = 4u,
	RightDown = 8u,
	RightUp = 0x10u,
	MiddleDown = 0x20u,
	MiddleUp = 0x40u,
	XButtonDown = 0x80u,
	XButtonUp = 0x100u,
	Wheel = 0x800u,
	HorzWheel = 0x1000u,
	MoveAbsolute = 0x8000u
}
