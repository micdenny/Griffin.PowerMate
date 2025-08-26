namespace Griffin.Audio;

internal interface IOSVolume
{
	float MasterVolume { get; }

	bool MasterMute { get; }
}
