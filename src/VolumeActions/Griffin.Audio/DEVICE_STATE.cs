namespace Griffin.Audio;

internal enum DEVICE_STATE : uint
{
	ACTIVE = 1u,
	UNPLUGGED = 2u,
	NOTPRESENT = 4u,
	MASK_ALL = 7u
}
