namespace Griffin.Appcasting;

public enum RESTART_Flags : uint
{
	None = 0u,
	NO_CRASH = 1u,
	NO_HANG = 2u,
	NO_PATCH = 4u,
	NO_REBOOT = 8u
}
