using System.Runtime.InteropServices;

namespace Griffin.PowerMate.iTunesActions;

[Guid("3D502ACA-B474-4640-A2A4-C149538345EC")]
internal enum ITPlayerState
{
	ITPlayerStateStopped,
	ITPlayerStatePlaying,
	ITPlayerStateFastForward,
	ITPlayerStateRewind
}
